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

    public float horizontal;
    public float speed;
    public float jumpForce;
    public float walljumpForce;
    public float downPower;

    bool lookingRight = true;

    bool noStamina = false;
    public float maxStamina = 100;
    public float stamina;

    public int maxHealth = 100;
    public int health;
    public bool isDead = false;
    Vector3 scale;


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
        if (Input.GetKeyDown(KeyCode.S) && !isGrounded() || Input.GetKeyDown(KeyCode.DownArrow) && !isGrounded())
        {
            Vector3 vel = rb.velocity;
            vel.y -= downPower;
            rb.velocity = vel;
        }

        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && !noStamina && isGrounded())
        {
            if(Mathf.Abs(horizontal) > 0.1 )
            {
                animator.SetBool("isRunning", true);
                speed = 250;
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

        //Zýplama
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        //Ayarlanabilir Zýplama Yüksekliði
        if(Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0|| Input.GetKeyUp(KeyCode.UpArrow) && rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);

        if (onWall())
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.gravityScale = 3;
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

    }

    private void Jump()
    {
        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }
        else if (onWall() && !isGrounded())
        {
            rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * walljumpForce, walljumpForce);
            animator.SetBool("isJumping", true);
        }
    }

    void FlipCharacter()
    {
        
        lookingRight = !lookingRight;
        scale = gameObject.transform.localScale;
        scale.x *= -1;
        gameObject.transform.localScale = scale;
    }

    void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(horizontal * Time.deltaTime * speed, rb.velocity.y, 0);
        if(isGrounded())
            animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if(!isDead && horizontal > 0 && lookingRight == false)
        {
            FlipCharacter();
        }

        if (!isDead && horizontal < 0 && lookingRight == true)
        {
            FlipCharacter();
        }
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

    }


    private void RegenerateStamina() {

        animator.SetBool("isRunning", false);
        speed = 125;

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
