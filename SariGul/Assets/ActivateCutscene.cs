using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ActivateCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private GameObject boss;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.SetActive(true);
            playableDirector.Play();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
