using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    Leaderboard leaderboard;
    public TextMeshProUGUI timerText;
    private float startTime;
    private bool isRunning = false;
    [SerializeField] bool debug = false;
    int minutes;
    int seconds;
    int nanoseconds;
    float elapsedTime;

    private void Start()
    {
        StartTimer();
        leaderboard = GetComponent<Leaderboard>();
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime = Time.time - startTime;
            minutes = (int)(elapsedTime / 60f);
            seconds = (int)(elapsedTime % 60f);
            float fractionalSeconds = elapsedTime - Mathf.Floor(elapsedTime);
            nanoseconds = (int)(fractionalSeconds * 100);
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, nanoseconds);
        }
        if (debug && isRunning)
        {
            StopTimer();
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
        debug = false;
    }

    public void StopTimer()
    {
        isRunning = false;
        leaderboard.AddEntry(minutes,seconds,nanoseconds,elapsedTime);
    }

    public void ResetTimer()
    {
        startTime = Time.time;
        timerText.text = "00:00";
    }
}