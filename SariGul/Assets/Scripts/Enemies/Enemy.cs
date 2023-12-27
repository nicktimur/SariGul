using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private CapsuleCollider2D boxCollider;
    public LayerMask playerLayer;
    public int maxHealth = 100;
    int currentHealth;
    public Animator anime;
    public HealthBar healthBar;
    public PlayerCombat playerCombat;


    private void Awake()
    {
        anime = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if(PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            //Attack
            cooldownTimer = 0;
            anime.SetTrigger("Attack");
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * -transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);


        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * -transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerCombat.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
        anime.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anime.SetBool("IsDead", true);

        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GameObject.Find("Enemy Soldier/Canvas Enemy").GetComponent<CanvasGroup>().alpha = 0;


    }

}
