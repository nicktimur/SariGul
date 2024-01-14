using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private int damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    [SerializeField] private AudioClip burnSound;
    public float cooldownTimer = Mathf.Infinity;
    [SerializeField] private float attackCooldown;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered; //when the trap gets triggered
    private bool active; //when the trap is active and can hurt the player

    private PlayerCombat playerCombat;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (playerCombat != null && active && cooldownTimer >= attackCooldown) 
        { 
            playerCombat.TakeDamage(damage);
            cooldownTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerCombat = collision.GetComponent<PlayerCombat>();

            if (!triggered)
                StartCoroutine(ActivateFiretrap());

            if (active && cooldownTimer >= attackCooldown)
            {
                collision.GetComponent<PlayerCombat>().TakeDamage(damage);
                cooldownTimer = 0;
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerCombat = null;
    }
    private IEnumerator ActivateFiretrap()
    {
        //turn the sprite red to notify the player and trigger the trap
        triggered = true;
        spriteRend.color = Color.red;

        //Wait for delay, activate trap, turn on animation, return color back to normal
        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white; //turn the sprite back to its initial color
        active = true;
        anim.SetBool("activated", true);
        SoundManager.instance.PlaySound(burnSound);

        //Wait until X seconds, deactivate trap and reset all variables and animator
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}