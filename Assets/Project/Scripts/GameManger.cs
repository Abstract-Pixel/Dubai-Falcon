using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    bool ispaused;
    [SerializeField] string sceneName;
    [SerializeField] GameObject losescreen;
    [SerializeField] GameObject winscreen;
    [SerializeField] GameObject PauseUI;

    private void Start()
    {
        losescreen.SetActive(false);
        PauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        PauseUI.SetActive(true);
        ispaused = true;
    }

    public void WinState()
    {
        winscreen.SetActive(true);
        Time.timeScale = 0;
        Leaderboard.instance.UpdateLeaderboardUI();
    }

    public void LoseState()
    {
        losescreen.SetActive(true);
        Time.timeScale = 0;
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
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Leaderboard.instance.SaveLeaderboard();
    }
}