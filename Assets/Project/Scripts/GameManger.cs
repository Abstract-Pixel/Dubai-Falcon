using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public static GameManger instance;
    bool ispaused;
    [SerializeField] bool main;
    [SerializeField] string sceneName;
    [SerializeField] GameObject losescreen;
    [SerializeField] GameObject winscreen;
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject leaderboardText;
    [SerializeField] TimeManager time;

    [SerializeField] UnityEvent OnGameWin;
    [SerializeField] UnityEvent OnGameLose;

    string loseSoundKey = "GameLose";
    string winSoundKey = "GameWin";

    private void Start()
    {
        time = GetComponent<TimeManager>();
        instance = this;
        losescreen?.SetActive(false);
        PauseUI?.SetActive(false);
        winscreen?.SetActive(false);
        leaderboardText?.SetActive(false);
        PlayerDeath.OnFalconDie += LoseState;
        WinCondition.OnAllHoopsCollected += WinState;
    }

    private void OnDisable()
    {
        PlayerDeath.OnFalconDie -= LoseState;
        WinCondition.OnAllHoopsCollected -= WinState;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !main)
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
        PauseUI?.SetActive(true);
        ispaused = true;
    }

    public void WinState()
    {
        if (!main)
        {
            Time.timeScale = 0;
            AudioManager.Instance.PlayAudio(winSoundKey);
        }
        winscreen?.SetActive(true);
        leaderboardText?.SetActive(true);
        time?.StopTimer();
        OnGameWin?.Invoke();
        Leaderboard.instance.UpdateLeaderboardUI();
        Leaderboard.instance.SaveLeaderboard();
        Debug.Log("LOL");
    }

    public void LoseState()
    {
        Time.timeScale = 0;
        losescreen?.SetActive(true);
        AudioManager.Instance.PlayAudio(loseSoundKey);
        OnGameLose?.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(false);
        ispaused = false;
    }

    public void RestartGame()
    {
        ResumeGame();
        Leaderboard.instance.SaveLeaderboard();
        losescreen?.SetActive(false);
        PauseUI?.SetActive(false);
        winscreen?.SetActive(false);
        leaderboardText?.SetActive(false);
        LoadGame();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Leaderboard.instance.SaveLeaderboard();
    }
}