using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    public StaminaBar staminaBar;
    public HealthBar healthBar;
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


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        staminaBar.setMaxStamina(maxStamina);
        healthBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {

        horizontal = Input.GetAxis("Horizontal");

        if (isGrounded())
            animator.SetFloat("Speed", Mathf.Abs(horizontal));

        //Flip player when moving left-right
        if (horizontal > 0.01f && !isDead)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal < -0.01f && !isDead)
            transform.localScale = Vector3.one;

        if (Input.GetKeyDown(KeyCode.S) && !isGrounded() || Input.GetKeyDown(KeyCode.DownArrow) && !isGrounded())
        {
            Vector3 vel = rb.velocity;
            vel.y -= downPower;
            rb.velocity = vel;
        }

        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && !noStamina && isGrounded())
        {
            if (Mathf.Abs(horizontal) > 0.1)
            {
                animator.SetBool("isRunning", true);    
                speed = runningSpeed;
                stamina -= 12 * Time.deltaTime;
                staminaBar.setStamina(stamina);
            }

        }
        else
        {
            RegenerateStamina();
        }

        if (isGrounded())
        {
            animator.SetBool("isJumping", false);
        }

        //Kalkan
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded() && stamina > 10)
        {
            ShieldOn();
            shieldOn = true;
            SoundManager.instance.PlaySound(jumpSound);
            stamina -= 20 * Time.deltaTime;
            staminaBar.setStamina(stamina);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || stamina < 10)
        {
            shieldOn = false;
            animator.SetBool("Shield", false);
            shield.ActivateShield();
        }

        //Zýplama
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        //Ayarlanabilir Zýplama Yüksekliði
        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0 || Input.GetKeyUp(KeyCode.UpArrow) && rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);

        if (onWall())
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.gravityScale = 3;
            if(!shieldOn)
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
        }

    }

    private bool ChestInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * chestRange * -transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * chestRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, chestLayer);

        chestName = hit.collider.gameObject.name;
        GameObject chest;
        chest = GameObject.Find(chestName);
        chestAnimator = chest.GetComponent<Animator>();
        chestCollider = chest.GetComponent<BoxCollider2D>();

        return hit.collider != null;
    }


    private void Jump()
    {
        if (coyoteCounter < 0 && !onWall()) return;

        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("isJumping", true);
            SoundManager.instance.PlaySound(jumpSound);
        }

        if (onWall())
        {
            WallJump();
        }
        else
        {
            if (isGrounded())
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            else
            {
                if (coyoteCounter > 0)
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            coyoteCounter = 0;
        }
    }

    public void ShieldTakeDamage(int damage)
    {
        stamina -= damage * 3;
        if (stamina < 0)
            stamina = 0;
        staminaBar.setStamina(stamina);
    }

    private void ShieldOn()
    {
        animator.SetBool("Shield", true);
        shield.ActivateShield();
        rb.velocity = Vector2.zero;
    }

    private void WallJump()
    {
        rb.AddForce(new Vector2(Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        SoundManager.instance.PlaySound(jumpSound);
    }



    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
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

        if (stamina < 100)
        {
            stamina += 10 * Time.deltaTime;
            staminaBar.setStamina(stamina);
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
            stamina = 100;
            staminaBar.setStamina(stamina);
        }
    }

}
