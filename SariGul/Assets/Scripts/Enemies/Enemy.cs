using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Saldýrý Parametreleri")]
    public int damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;

    [Header("Collider Parametreleri")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private CapsuleCollider2D boxCollider;

    [Header("Player Layer")]
    public float cooldownTimer = Mathf.Infinity;
    [SerializeField] public LayerMask playerLayer;
    public int maxHealth = 100;
    int currentHealth;
    public Animator anime;
    public HealthBar healthBar;
    public PlayerCombat playerCombat;
    public Player player;
    public string enemyType;
    private EnemyPatrol enemyPatrol;
    public CloseEnemy closeEnemy;
    public RangedEnemy rangedEnemy;


    private void Awake()
    {
        anime = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
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
        if(!player.isDead && PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            //Attack
            cooldownTimer = 0;
            anime.SetTrigger("Attack");
        }

        if(enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }

        if (player.isDead)
        {
            anime.SetBool("PlayerDead", true);
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
            anime.SetBool("IsDead", true);
            if (enemyType == "Close")
                closeEnemy.Die();
            else if (enemyType == "Ranged")
                rangedEnemy.Die();
        }
    }

}
