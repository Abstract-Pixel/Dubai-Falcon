using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LeaderboardEntryData
{
    public string playerName;
    public float elapsedTime;
    public int minutes;
    public int seconds;
    public int nanoseconds;
}

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntryData> entries = new List<LeaderboardEntryData>();
}

public static class LeaderboardSaveLoad
{
    public static void SaveLeaderboard(List<LeaderboardEntry> leaderboardEntries)
    {
        LeaderboardData data = new LeaderboardData();
        foreach (LeaderboardEntry entry in leaderboardEntries)
        {
            LeaderboardEntryData entryData = new LeaderboardEntryData
            {
                playerName = entry.playerName,
                elapsedTime = entry.elapsedTime,
                minutes = entry.minutes,
                seconds = entry.seconds,
                nanoseconds = entry.nanoseconds
            };
            data.entries.Add(entryData);
        }

        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        File.WriteAllText(path, json);
        Debug.Log("Leaderboard saved to " + path);
    }

    public static List<LeaderboardEntry> LoadLeaderboard()
    {
        List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
        string path = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
            foreach (LeaderboardEntryData entryData in data.entries)
            {
                LeaderboardEntry entry = new LeaderboardEntry
                {
                    playerName = entryData.playerName,
                    elapsedTime = entryData.elapsedTime,
                    minutes = entryData.minutes,
                    seconds = entryData.seconds,
                    nanoseconds = entryData.nanoseconds
                };
                leaderboardEntries.Add(entry);
            }
            Debug.Log("Leaderboard loaded from " + path);
        }
        else
        {
            Debug.Log("No leaderboard file found at " + path);
        }
        return leaderboardEntries;
    }
}