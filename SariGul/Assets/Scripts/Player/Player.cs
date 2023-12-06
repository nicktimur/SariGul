using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    public StaminaBar staminaBar;
    public HealthBar healthBar;

    public float horizontal;
    public float speed;
    bool jump = true;
    public float jumpForce;
    public float downPower;

    bool lookingRight = true;
    bool isJumping = false;

    bool noStamina = false;
    public float maxStamina = 100;
    public float stamina;

    public int maxHealth = 100;
    public int health;

    private float jumpTimer = 0f;
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
        if (Input.GetKeyDown(KeyCode.W) && jump == true || Input.GetKeyDown(KeyCode.UpArrow) && jump == true)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            jump = false;
        }

        if (Input.GetKeyDown(KeyCode.S) && jump == false || Input.GetKeyDown(KeyCode.DownArrow) && jump == false)
        {
            Vector3 vel = rb.velocity;
            vel.y -= downPower;
            rb.velocity = vel;
        }

        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && !noStamina && jump)
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


        if (jump == false)
        {
            isJumping = true;
        }
        else if (jump == true)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        if (isJumping)
        {
            jumpTimer += Time.deltaTime;

            // Eðer 0.1 saniyeden fazla süre boyunca isJumping true ise animasyonu baþlat
            if (jumpTimer > 0.1f)
            {
                animator.SetBool("isJumping", true);
                jumpTimer = 0f;
            }
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
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if(horizontal > 0 && lookingRight == false)
        {
            FlipCharacter();
        }

        if (horizontal < 0 && lookingRight == true)
        {
            FlipCharacter();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jump = false;
        }

    }

}
