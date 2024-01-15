using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator animator;
    public StaminaBar staminaBar;
    public PlayerHealthBar healthBar;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Shield shield;

    [SerializeField] private float chestRange;
    [SerializeField] private float colliderDistance;
    [SerializeField] public LayerMask chestLayer;

    private Animator chestAnimator;
    private BoxCollider2D chestCollider;
    private string chestName;


    [SerializeField] private float coyoteTime; // Karakterin havada asýlý kalýrken zýplayabilceði süre
    private float coyoteCounter;

    public float horizontal;
    private float speed;
    public float walkingSpeed;
    public float runningSpeed;
    public float jumpForce;

    [Header("Wall jumping")]
    public float walljumpForce;
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    public float downPower;

    public bool shieldOn = false;

    bool noStamina = false;
    public float maxStamina = 100;
    public float stamina;

    public int maxHealth = 100;
    public int health;
    public bool isDead = false;
    Vector3 scale;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private float rangedAttackCooldown;
    [SerializeField] private Animator heartAnimator;
    [SerializeField] private Animator shieldAnimator;
    [SerializeField] private Animator canvasAnimator;

    private Inventory inventory;
    private GameObject chest;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Mesafeli Saldýrý")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip shieldSound;
    public LayerMask easterEggLayer;
    [SerializeField] private AudioClip mustafaSound;
    public bool gamePaused;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        staminaBar.setMaxStamina(maxStamina);
        healthBar.setMaxHealth(maxHealth);

        inventory = new Inventory();
        uiInventory.SetInventory(inventory);

    }

    // Update is called once per frame
    void Update()
    {

        if(health <= 20)
        {
            canvasAnimator.SetBool("InDanger", true);
        }
        else
        {
            canvasAnimator.SetBool("InDanger", false);
        }

        if (!gamePaused)
        {

            cooldownTimer += Time.deltaTime;
            horizontal = Input.GetAxis("Horizontal");

            if (isGrounded())
                animator.SetFloat("Speed", Mathf.Abs(horizontal));

            //Flip player when moving left-right
            if (horizontal > 0.01f && !isDead && !shieldOn)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            else if (horizontal < -0.01f && !isDead && !shieldOn)
            {
                transform.localScale = Vector3.one;

            }

            if (Input.GetKeyDown(KeyCode.S) && !isGrounded() || Input.GetKeyDown(KeyCode.DownArrow) && !isGrounded())
            {
                Vector3 vel = rb.velocity;
                vel.y -= downPower;
                rb.velocity = vel;
            }

            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && !noStamina)
            {
                if (Mathf.Abs(horizontal) > 0.1)
                {
                    if (isGrounded())
                    {
                        animator.SetBool("isRunning", true);
                    }
                    speed = runningSpeed;
                    stamina -= 12 * Time.deltaTime;
                    staminaBar.setStamina(stamina, maxStamina);
                }

            }
            else
            {
                RegenerateStamina();
            }

            if (shieldOn)
            {
                stamina -= 0.02f;
                staminaBar.setStamina(stamina, maxStamina);
            }

            if (isGrounded() && !onWall())
            {
                animator.SetBool("isJumping", false);
            }
            else
            {
                animator.SetBool("isJumping", true);
            }

            //Kalkan
            if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded() && stamina > 10)
            {
                shieldOn = true;
                SoundManager.instance.PlaySound(jumpSound);
                stamina -= 20 * Time.deltaTime;
                staminaBar.setStamina(stamina, maxStamina);
                ShieldOn();
            }

            if (Input.GetKeyUp(KeyCode.LeftControl) || stamina < 10)
            {
                shieldOn = false;
                animator.SetBool("Shield", false);
                shield.DeactivateShield();
            }

            //Zýplama
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                animator.SetBool("isJumping", true);
                Jump();
            }

            //Ayarlanabilir Zýplama Yüksekliði
            if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0 || Input.GetKeyUp(KeyCode.UpArrow) && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
                animator.SetBool("isJumping", true);
            }

            if (onWall())
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.gravityScale = 3;
                if (!shieldOn)
                {
                    rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                }
                if (isGrounded())
                {
                    coyoteCounter = coyoteTime;
                }
                else
                {
                    coyoteCounter -= Time.deltaTime;
                }
            }

            //Sandýk açma
            if (Input.GetKey(KeyCode.Z) && isGrounded() && ChestInSight())
            {
                chestAnimator.SetBool("IsOpened", true);
                chestCollider.enabled = false;

                inventory.AddItem(chest.GetComponent<Chest>().GetItem());
            }
        }
    }

    public void DisableControls()
    {
        gamePaused = true;
    }

    public void EnableControls()
    {
        gamePaused = false;
    }

    public void AddMaxHealth(int extraHealth)
    {
        maxHealth += extraHealth;
        health += extraHealth;
        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(health, maxHealth);
    }

    public void AddMaxStamina(int extraStamina)
    {
        maxStamina += extraStamina;
        stamina += extraStamina;
        staminaBar.setMaxStamina(maxStamina);
        staminaBar.setStamina(stamina, maxStamina);
    }

    private void RangedAttack()
    {
        if (cooldownTimer > rangedAttackCooldown)
        {
            inventory.RemoveItem(new Item { itemType = Item.ItemType.Fireball, amount = 1 }); ;
            cooldownTimer = 0;
            SoundManager.instance.PlaySound(fireballSound);
            fireballs[FindFireball()].transform.position = firePoint.position;
            fireballs[FindFireball()].GetComponent<PlayerProjectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    public void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                health += 25;
                if (health > maxHealth) health = maxHealth;
                healthBar.setHealth(health, maxHealth);
                heartAnimator.SetTrigger("Heal");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                break;

            case Item.ItemType.Fireball:
                RangedAttack();
                break;

        }
    }

    private bool ChestInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * chestRange * -transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * chestRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, chestLayer);
        if (hit.collider != null)
        {
            chestName = hit.collider.gameObject.name;
            chest = GameObject.Find(chestName);
            chestAnimator = chest.GetComponent<Animator>();
            chestCollider = chest.GetComponent<BoxCollider2D>();
        }

        return hit.collider != null;
    }

    private void Jump()
    {
        animator.SetBool("isJumping", true);
        if (coyoteCounter < 0 && !onWall()) return;

        if (onWall())
        {
            WallJump();
        }
        else
        {
            if (isGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                SoundManager.instance.PlaySound(jumpSound);
                animator.SetBool("isJumping", true);
            }
            else
            {
                if (coyoteCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    SoundManager.instance.PlaySound(jumpSound);
                    animator.SetBool("isJumping", true);
                }

            }
            coyoteCounter = 0;
        }
    }
    private void WallJump()
    {
        rb.AddForce(new Vector2(Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        animator.SetBool("isJumping", true);
        SoundManager.instance.PlaySound(jumpSound);
    }

    public void ShieldTakeDamage(int damage)
    {
        shieldAnimator.SetTrigger("shield");
        SoundManager.instance.PlaySound(shieldSound);
        stamina -= damage;
        if (stamina < 0)
            stamina = 0;
        staminaBar.setStamina(stamina, maxStamina);
    }

    private void ShieldOn()
    {
        animator.SetBool("Shield", true);
        shield.ActivateShield();
        rb.velocity = Vector2.zero;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D easterEggHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, easterEggLayer);
        if(easterEggHit.collider != null)
        {
            MusicSource.instance.PlaySound(mustafaSound);
        }

        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(-transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * chestRange * -transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * chestRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

    }

    private void RegenerateStamina()
    {

        animator.SetBool("isRunning", false);
        speed = walkingSpeed;
        if (!shieldOn)
        {
            if (stamina < maxStamina)
            {
                stamina += 10 * Time.deltaTime;
                staminaBar.setStamina(stamina, maxStamina);
                if (stamina > 10)
                {
                    noStamina = false;
                }
                else
                {
                    noStamina = true;
                }
            }
            else
            {
                stamina = maxStamina;
                staminaBar.setStamina(stamina, maxStamina);
            }
        }
    }

}
