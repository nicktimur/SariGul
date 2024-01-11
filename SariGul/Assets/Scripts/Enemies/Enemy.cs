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
    [SerializeField] private float backRange;

    [Header("Collider Parametreleri")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private CapsuleCollider2D boxCollider;
    [SerializeField] private float backColliderDistance;


    [Header("Player Layer")]
    public float cooldownTimer = Mathf.Infinity;
    [SerializeField] public LayerMask playerLayer;
    public int maxHealth = 100;
    int currentHealth;
    private Animator anime;
    public HealthBar healthBar;
    private PlayerCombat playerCombat;
    private Player player;
    public string enemyType;
    [SerializeField] private EnemyPatrol enemyPatrol;
    public CloseEnemy closeEnemy;
    public RangedEnemy rangedEnemy;

    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip AttackSound;
    public Transform AttackPoint;
    public float attackRange = 0.5f;



    private void Awake()
    {
        anime = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        player = GameObject.Find("PlayerKnight").GetComponent<Player>();
        playerCombat = GameObject.Find("PlayerKnight").GetComponent<PlayerCombat>();
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

        if (!player.isDead &&  PlayerInBack())
        {
            enemyPatrol.TurnBack();
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

    private bool PlayerInBack()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * backRange * -transform.localScale.x * backColliderDistance,
            new Vector3(boxCollider.bounds.size.x * backRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);


        return hit.collider != null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * -transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * backRange * -transform.localScale.x * backColliderDistance,
            new Vector3(boxCollider.bounds.size.x * backRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        SoundManager.instance.PlaySound(AttackSound);
        Collider2D[] hitplayer = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, playerLayer);

        foreach (Collider2D playerr in hitplayer)
        {
            if (PlayerInSight())
            {
                if(!(player.shieldOn && player.transform.localScale.x != this.transform.localScale.x)) //Kalkan açýk deðilse
                {
                    playerr.GetComponent<PlayerCombat>().TakeDamage(damage);
                }
                else
                {
                    player.ShieldTakeDamage(damage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
            return;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        SoundManager.instance.PlaySound(hurtSound);
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
