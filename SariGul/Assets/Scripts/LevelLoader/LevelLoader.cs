using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public Animator canvas;
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        if(canvas != null)
        {
        canvas.SetTrigger("FadeIn");
        }

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(levelIndex);

        if (canvas != null)
        {
            canvas.SetTrigger("FadeOut");
        }
    }
}
