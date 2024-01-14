using UnityEngine;

public class CloseEnemy : MonoBehaviour


{

    [SerializeField] private Behaviour[] components;
    [SerializeField] private AudioClip dieSound;
    private Player player;

    private void Awake()
    {
        player = GameObject.Find("PlayerKnight").GetComponent<Player>();
    }

    public void Die()
    {
        SoundManager.instance.PlaySound(dieSound);
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        player.AddMaxHealth(10);

        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }

    }
}
