using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        Destroy(GameObject.Find("Canvas"));
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("Main Camera"));
        Destroy(GameObject.Find("LevelLoader"));
    }
}
