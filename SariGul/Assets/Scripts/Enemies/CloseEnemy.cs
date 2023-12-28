using UnityEngine;

public class CloseEnemy : MonoBehaviour


{

    [SerializeField] private Behaviour[] components;
    public void Die()
    {

        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GameObject.Find("Enemy Soldier/Canvas Enemy").GetComponent<CanvasGroup>().alpha = 0;

        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }

    }
}
