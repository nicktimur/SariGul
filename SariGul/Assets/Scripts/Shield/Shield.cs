
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private Behaviour[] components;

    public void ActivateShield()
    {
        gameObject.SetActive(true);
        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }

    }

    private void DeactivateShield()
    {
        gameObject.SetActive(false);
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }
    }
}
