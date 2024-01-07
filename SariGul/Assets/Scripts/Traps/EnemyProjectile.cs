using Cainos.PixelArtPlatformer_VillageProps;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public string rangeType;
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;
    private PlayerCombat playerCombat;
    private Player player;
    [SerializeField] private Enemy enemy;
    private Enemy damagedEnemy; 

    private bool hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        string playerName = "PlayerKnight";
        player = GameObject.Find(playerName).GetComponent<Player>();
        playerCombat = GameObject.Find(playerName).GetComponent<PlayerCombat>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        Boom(collision, enemy.damage); //Execute logic from parent script first
        coll.enabled = false;

        if (anim != null)
            anim.SetTrigger("explode"); //When the object is a fireball explode it
        else
            gameObject.SetActive(false); //When this hits any object deactivate arrow
    }

    protected void Boom(Collider2D collision, int damage)
    {
        if (collision.tag == "Player" && rangeType == "dmg")
            playerCombat.TakeDamage(damage);
        else if (collision.tag == "Player" && rangeType == "ptn")
            playerCombat.TakeStaminaDamage(damage);
        else if (collision.tag == "Shield")
            player.ShieldTakeDamage(damage);
        else if( collision.tag == "Enemy")
        {
            damagedEnemy = collision.gameObject.GetComponent<Enemy>();
            damagedEnemy.TakeDamage(damage);
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}