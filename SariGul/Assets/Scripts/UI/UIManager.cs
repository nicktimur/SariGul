
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header ("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    [Header("MainMenu")]
    [SerializeField] private GameObject mainMenu;
    private Animator anim;
    private Player player;

    private LevelLoader levelLoader;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if(SceneManager.GetActiveScene().buildIndex != 0)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        levelLoader = FindFirstObjectByType<LevelLoader>().GetComponent<LevelLoader>();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
            }
        }
    }

    public void StartGame()
    {
        levelLoader.LoadNextLevel();
        anim.SetTrigger("Start");
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PauseGame(bool stasus)
    {
        pauseScreen.SetActive(stasus);
        if(stasus)
        {
            Time.timeScale = 0;
            MusicSource.instance.PauseSound();
            player.gamePaused = true;
        }
        else
        {
            Time.timeScale = 1;
            MusicSource.instance.UnpauseSound();
            player.gamePaused = false;
        }
    } 

    public void SoundVolume()
    {

    }
    public void MusicVolume() { }
}
