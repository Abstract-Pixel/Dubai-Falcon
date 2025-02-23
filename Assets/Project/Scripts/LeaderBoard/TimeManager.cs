using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    [SerializeField] string startText;
    float startTime;
    bool isRunning = false;
    int minutes;
    int seconds;
    int nanoseconds;
    float elapsedTime;
    [SerializeField]MMF_Player feedbacks;
    [SerializeField] int feedbackTrigger;

    private void Start()
    {
        StartTimer();
        InvokeRepeating(nameof(TimerFeedbacks), 0, feedbackTrigger);
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
            timerText.text = startText + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, nanoseconds);


        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
        Leaderboard.instance.AddEntry(minutes,seconds,nanoseconds,elapsedTime);
    }

    public void ResetTimer()
    {
        startTime = Time.time;
        timerText.text = "00:00";
    }

    void TimerFeedbacks()
    {
        if(isRunning)
        {
            feedbacks?.PlayFeedbacks();
        }
       
    }
}