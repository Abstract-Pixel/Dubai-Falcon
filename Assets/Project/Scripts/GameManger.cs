using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public static GameManger instance;
    bool ispaused;
    [SerializeField] string sceneName;
    [SerializeField] GameObject losescreen;
    [SerializeField] GameObject winscreen;
    [SerializeField] GameObject PauseUI;
    [SerializeField] TimeManager time;

    private void Start()
    {
        time = GetComponent<TimeManager>();
        instance = this;
        //losescreen?.SetActive(false);
        // PauseUI?.SetActive(false);

        PlayerDeath.OnFalconDie+=LoseState;
        WinCondition.OnAllHoopsCollected+=WinState;
    }

    private void OnDisable()
    {
        PlayerDeath.OnFalconDie -=LoseState;
        WinCondition.OnAllHoopsCollected-=WinState;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ispaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        //PauseUI?.SetActive(true);
        ispaused = true;
    }

    public void WinState()
    {
        Time.timeScale = 0;
        // winscreen?.SetActive(true);
        time?.StopTimer();
        Leaderboard.instance.UpdateLeaderboardUI();
        Leaderboard.instance.SaveLeaderboard();
    }

    public void LoseState()
    {
        Time.timeScale = 0;
        //losescreen?.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        //PauseUI.SetActive(false);
        ispaused = false;
    }

    public void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Leaderboard.instance.SaveLeaderboard();
    }
}