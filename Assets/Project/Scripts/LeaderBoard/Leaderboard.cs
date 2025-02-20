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
    public static Leaderboard instance;
    public Transform leaderboardContainer;
    public GameObject entryPrefab;
    public TextMeshProUGUI extraEntryText;
    [SerializeField] List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    private LeaderboardEntry lastAddedEntry;

    private void Start()
    {
        instance = this;
    }

    public void AddEntry(int minutes, int seconds, int nanoseconds, float totalTime)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry();
        newEntry.elapsedTime = totalTime;
        newEntry.minutes = minutes;
        newEntry.seconds = seconds;
        newEntry.nanoseconds = nanoseconds;
        leaderboardEntries.Add(newEntry);
        lastAddedEntry = newEntry;
        newEntry.entryObject = entryPrefab;
    }

    public void UpdateLeaderboardUI()
    {
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }
        if (extraEntryText != null)
            extraEntryText.text = "";

        leaderboardEntries.Sort((a, b) => a.elapsedTime.CompareTo(b.elapsedTime));
        entryPrefab.SetActive(true);
        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            LeaderboardEntry entry = leaderboardEntries[i];
            entry.entryObject = Instantiate(entryPrefab, leaderboardContainer, false);
            entry.entryObject.transform.SetSiblingIndex(i);
            string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", entry.minutes, entry.seconds, entry.nanoseconds);
            TextMeshProUGUI entryText = entry.entryObject.GetComponent<TextMeshProUGUI>();

            if (entryText != null)
            {
                entryText.text = string.Format("{0}. {1}", i + 1, formattedTime);
                entryText.color = Color.white;
            }

            if (i >= 10)
            {
                entry.entryObject.SetActive(false);
                if (extraEntryText != null)
                {
                    extraEntryText.text += string.Format("{0}. {1}\n", i + 1, formattedTime);
                }
            }
        }
    }

    public void SaveLeaderboard()
    {
        LeaderboardSaveLoad.SaveLeaderboard(leaderboardEntries);
    }

    public void LoadLeaderboard()
    {
        leaderboardEntries = LeaderboardSaveLoad.LoadLeaderboard();
        UpdateLeaderboardUI();
    }
}