using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Saldýrý Parametreleri")]
    [SerializeField] public int damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;

    [Header("Mesafeli Saldýrý")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider Parametreleri")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private CapsuleCollider2D boxCollider;

    [Header("Player Layer")]
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] public LayerMask playerLayer;
    public int maxHealth = 100;
    int currentHealth;
    public Animator anime;
    public HealthBar healthBar;
    public PlayerCombat playerCombat;
    public Player player;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (!player.isDead && PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            //Attack
            cooldownTimer = 0;
            anime.SetTrigger("Attack");
        }

        if (enemyPatrol != null)
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

    private void RangedAttack()
    {
        cooldownTimer = 0;
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
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


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
        anime.SetTrigger("Hurt");

        if (currentHealth <= 0)
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
        GameObject.Find("Enemy Priest/Canvas Enemy").GetComponent<CanvasGroup>().alpha = 0;

    }
}
