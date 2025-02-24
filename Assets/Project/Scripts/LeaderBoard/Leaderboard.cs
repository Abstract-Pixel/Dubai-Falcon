using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
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
        LoadLeaderboard();
    }

    public void AddEntry(int minutes, int seconds, int nanoseconds, float totalTime)
    {
        string playerName = (PlayerData.Instance != null) ? PlayerData.Instance.playerName : "Falco";

        LeaderboardEntry newEntry = new LeaderboardEntry();
        newEntry.playerName = playerName; // Save player's name with their time
        newEntry.elapsedTime = totalTime;
        newEntry.minutes = minutes;
        newEntry.seconds = seconds;
        newEntry.nanoseconds = nanoseconds;

        leaderboardEntries.Add(newEntry);
        lastAddedEntry = newEntry;
    }

    public void UpdateLeaderboardUI()
    {
        // Clear existing UI elements
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }
        if (extraEntryText != null)
        {
            extraEntryText.text = "";
            extraEntryText.color = Color.white; // Reset extra text color
        }

        // Sort entries by elapsed time (ascending)
        leaderboardEntries.Sort((a, b) => a.elapsedTime.CompareTo(b.elapsedTime));

        // Determine the index of the player's last entry (if any)
        int lastEntryIndex = -1;
        if (lastAddedEntry != null)
        {
            lastEntryIndex = leaderboardEntries.IndexOf(lastAddedEntry);
        }

        int totalSlots = 10;
        for (int i = 0; i < totalSlots; i++)
        {
            // Always instantiate a UI element for each slot (even if empty)
            GameObject entryObj = Instantiate(entryPrefab, leaderboardContainer, false);
            entryObj.transform.SetSiblingIndex(i);
            TextMeshProUGUI entryText = entryObj.GetComponent<TextMeshProUGUI>();

            // If an entry exists for this rank, display its details
            if (i < leaderboardEntries.Count)
            {
                LeaderboardEntry entry = leaderboardEntries[i];
                string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", entry.minutes, entry.seconds, entry.nanoseconds);
                if (entryText != null)
                {
                    entryText.text = string.Format("{0}. {1}", i + 1, formattedTime);
                    // Highlight if this is the player's last entry
                    entryText.color = (entry == lastAddedEntry) ? Color.yellow : Color.white;
                }
            }
            else
            {
                // No entry available for this rank; just display the rank number
                if (entryText != null)
                {
                    entryText.text = string.Format("{0}.", i + 1);
                    entryText.color = Color.white;
                }
            }
        }

        // If the player's last entry exists and isn't in the top 10, display it separately
        if (lastAddedEntry != null && lastEntryIndex >= totalSlots && extraEntryText != null)
        {
            string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", lastAddedEntry.minutes, lastAddedEntry.seconds, lastAddedEntry.nanoseconds);
            extraEntryText.text = string.Format("Your Time: {0}. {1}", lastEntryIndex + 1, formattedTime);
            extraEntryText.color = Color.yellow;
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