using UnityEngine;

public class CloseEnemy : MonoBehaviour


{

    [SerializeField] private Behaviour[] components;
    [SerializeField] private AudioClip dieSound;

    public void Die()
    {
        SoundManager.instance.PlaySound(dieSound);
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }

    }
}
