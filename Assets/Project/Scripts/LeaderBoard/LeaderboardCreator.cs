using UnityEngine;

public class LeaderboardCreator : MonoBehaviour
{
    void Start()
    {
        Leaderboard.instance.LoadLeaderboard();
        Leaderboard.instance.UpdateLeaderboardUI();
    }

}
