using UnityEngine;

public class CloseEnemy : MonoBehaviour


{

    [SerializeField] private Behaviour[] components;
    public void Die()
    {

        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }

    }
}
