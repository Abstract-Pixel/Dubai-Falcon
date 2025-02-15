using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public Transform leaderboardContainer;
    public GameObject entryPrefab;
    [SerializeField]  TextMeshProUGUI textMesh;
    [SerializeField] List<GameObject> entryObjects = new List<GameObject>();

    public void AddEntry(float elapsedTime)
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f); float fractionalSeconds = elapsedTime - Mathf.Floor(elapsedTime);
        int nanoseconds = (int)(fractionalSeconds * 1000);
        string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, nanoseconds);
        GameObject newEntry = Instantiate(entryPrefab, leaderboardContainer);
        if (textMesh != null)
        {
            textMesh.text = formattedTime;
        }
        entryObjects.Add(newEntry);
        HighlightLastEntry();
    }

    private void HighlightLastEntry()
    {
        foreach (GameObject entry in entryObjects)
        {
            TextMeshProUGUI textMesh = entry.GetComponent<TextMeshProUGUI>();
            if (textMesh != null)
            {
                textMesh.color = Color.white;
            }
        }
        if (entryObjects.Count > 0)
        {
            TextMeshProUGUI lastTextMesh = entryObjects[entryObjects.Count - 1].GetComponent<TextMeshProUGUI>();
            if (lastTextMesh != null)
            {
                lastTextMesh.color = Color.yellow;
            }
        }
    }
}