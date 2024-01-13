using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
	public int attackDamage = 20;
	public int enragedAttackDamage = 40;
	private Player player;
    [SerializeField] private AudioClip AttackSound;

    public Vector3 attackOffset;
	public float attackRange;
	public LayerMask attackMask;
	private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
		player = GameObject.Find("PlayerKnight").GetComponent<Player>();
    }

    private void Update()
    {
        if (player.isDead)
		{
			animator.SetBool("PlayerDied", true);
		}
    }

    public void Attack()
	{
		if(player.isDead) return;

        SoundManager.instance.PlaySound(AttackSound);
        Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

		Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
		if (colInfo != null)
		{
            if (!(player.shieldOn && player.transform.localScale.x != this.transform.localScale.x)) //Kalkan açık değilse
			{
                colInfo.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
			}
			else
			{
				player.ShieldTakeDamage(attackDamage);
            }
		}
	}

	public void EnragedAttack()
	{
        if (player.isDead) return;

        SoundManager.instance.PlaySound(AttackSound);
        Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

		Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
		if (colInfo != null)
		{
            if (!(player.shieldOn && player.transform.localScale.x != this.transform.localScale.x)) //Kalkan açık değilse
            {
                colInfo.GetComponent<PlayerCombat>().TakeDamage(enragedAttackDamage);
            }
            else
            {
                player.ShieldTakeDamage(enragedAttackDamage);
            }
        }
	}

	void OnDrawGizmosSelected()
	{
		Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

		Gizmos.DrawWireSphere(pos, attackRange);
	}
}
