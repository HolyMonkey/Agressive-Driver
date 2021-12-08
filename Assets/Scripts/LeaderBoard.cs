using UnityEngine;
using YandexGames;

public class LeaderBoard : MonoBehaviour
{
    private void Start()
    {
        if (PlayerAccount.IsAuthorized)
        {
            Leaderboard.GetEntries("PlaytestBoard", (result) =>
            {
                // Use it
                Debug.Log($"My rank = {result.userRank}");
                foreach (var entry in result.entries)
                {
                    Debug.Log(entry.player.publicName + " " + entry.score);
                }
            });
        }
    }
}
