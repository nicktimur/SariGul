using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    private LevelLoader levelLoader;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
            levelLoader.LoadNextLevel();

            Transform player = collision.GetComponent<Player>().transform;
            Transform camera = GameObject.Find("Main Camera").transform;
            player.position = new Vector3(20.65637f, 6.800001f, 0.1985453f);
            camera.position = new Vector3(21.71f, 8.9f, -5);

        }

    }
}
