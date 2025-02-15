using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class LeaderboardEntry
{
    public GameObject entryObject;
    public float elapsedTime;
    public int minutes;
    public int seconds;
    public int nanoseconds;
}

public class Leaderboard : MonoBehaviour
{
    public Transform leaderboardContainer;
    public GameObject entryPrefab;
    [SerializeField] List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();

    public void AddEntry(int minutes, int seconds, int nanoseconds, float totalTime)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry();
        newEntry.elapsedTime = totalTime;
        newEntry.minutes = minutes;
        newEntry.seconds = seconds;
        newEntry.nanoseconds = nanoseconds;
        newEntry.entryObject = Instantiate(entryPrefab, leaderboardContainer, false);
        leaderboardEntries.Add(newEntry);
        leaderboardEntries.Sort((a, b) => a.elapsedTime.CompareTo(b.elapsedTime));
        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            LeaderboardEntry entry = leaderboardEntries[i];
            entry.entryObject.transform.SetSiblingIndex(i);
            string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", entry.minutes, entry.seconds, entry.nanoseconds);
            TextMeshProUGUI entryText = entry.entryObject.GetComponent<TextMeshProUGUI>();
            if (entryText != null)
            {
                entryText.text = string.Format("{0}. {1}", i + 1, formattedTime);
                entryText.color = Color.white;
            }
        }
        TextMeshProUGUI newEntryText = newEntry.entryObject.GetComponent<TextMeshProUGUI>();
        if (newEntryText != null)
        {
            newEntryText.color = Color.yellow;
        }
    }
}