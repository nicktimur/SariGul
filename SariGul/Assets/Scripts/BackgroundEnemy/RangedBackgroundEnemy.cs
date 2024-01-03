using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBackgroundEnemy : MonoBehaviour
{
    [Header("Mesafeli Saldýrý")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    [SerializeField] private AudioClip fireballSound;
    public float cooldownTimer = Mathf.Infinity;
    [SerializeField] private float attackCooldown;
    public Animator anime;

    private void Awake()
    {
        anime = GetComponent<Animator>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown)
        {
            //Attack
            cooldownTimer = 0;
            anime.SetTrigger("Attack");
        }
    }

    private void RangedAttack()
    {
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<BackgroundEnemyProjectile>().ActivateProjectile();
        WarSound.instance.PlaySound(fireballSound);
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
}
