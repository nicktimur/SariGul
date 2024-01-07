using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator anime;
    public Transform AttackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public LayerMask bossLayer;
    public int attackDamage = 20;
    private float attackRate = 2f;
    private UIManager uiManager;

    float nextAttackTime = 0f;
    private Player player;
    public StaminaBar staminaBar;
    public HealthBar healthBar;

    [SerializeField] private AudioClip AttackSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private Behaviour[] components;

    private void Awake()
    {
        player = GetComponent<Player>();
        uiManager = FindFirstObjectByType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) && player.stamina > 0 && !player.shieldOn)
            {
                anime.SetTrigger("Attacking");
                player.stamina -= 30;
                staminaBar.setStamina(player.stamina);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

    }

    void Attack()
    {
        SoundManager.instance.PlaySound(AttackSound);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayers);
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, bossLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        foreach (Collider2D boss in hitBoss)
        {
            boss.GetComponent<BossHealth>().TakeDamage(attackDamage);
        }

    }

    public void TakeDamage(int damage)
    {
        SoundManager.instance.PlaySound(hurtSound);
        player.health -= damage;
        anime.SetTrigger("Hurt");
        healthBar.setHealth(player.health);
        if(player.health <= 0)
        {
            KillPlayer();
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        TakeDamage(damage);
        player.stamina -= damage * 5;
        staminaBar.setStamina(player.stamina);
    }

    void KillPlayer()
    {
        SoundManager.instance.PlaySound(dieSound);
        anime.SetBool("Died", true);
        player.isDead = true;

        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }

        MusicSource.instance.StopSound();
        uiManager.GameOver();
    }

    void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
            return;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }
}
