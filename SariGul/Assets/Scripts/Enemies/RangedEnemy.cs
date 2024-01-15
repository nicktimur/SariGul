using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Mesafeli Saldýrý")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    [SerializeField] private Behaviour[] components;

    public Enemy enemy;


    [SerializeField]
    private AudioClip fireballSound;
    [SerializeField] private AudioClip dieSound;
    private Player player;

    private void Awake()
    {
        player = GameObject.Find("PlayerKnight").GetComponent<Player>();
    }


    private void RangedAttack()
    {
        enemy.cooldownTimer = 0;
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
        SoundManager.instance.PlaySound(fireballSound);
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


    public void Die()
    {
        SoundManager.instance.PlaySound(dieSound);
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        player.AddMaxHealth(20);
        player.AddMaxStamina(10);

        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }
    }
}
