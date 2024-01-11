using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    private LevelLoader levelLoader;
    private Transform player;
    private Transform cameraTransform;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
            levelLoader.LoadNextLevel();


            player = collision.GetComponent<Player>().transform;
            cameraTransform = GameObject.Find("Main Camera").transform;

            StartCoroutine(SetPosition());

        }

    }

    IEnumerator SetPosition()
    {
        yield return new WaitForSeconds(1);
        player.position = new Vector3(20.65637f, 6.800001f, 0.1985453f);
        cameraTransform.position = new Vector3(21.71f, 8.9f, -5);
    }
}
