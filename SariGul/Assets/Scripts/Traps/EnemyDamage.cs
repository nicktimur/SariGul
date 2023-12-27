using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected int damage;
    public PlayerCombat playerCombat;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerCombat.TakeDamage(damage);
    }
}