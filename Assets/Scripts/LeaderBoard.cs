using UnityEngine;
using YandexGames;
using System.Linq;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    private TMP_Text _playerScore;

    private void Start()
    {
        if (PlayerAccount.IsAuthorized)
        {
            Leaderboard.GetEntries("PlaytestBoard", (result) =>
            {
                var filterResult = result.entries.Where(result => result.score > 0).OrderByDescending(result => result.score);
                foreach (var entry in filterResult)
                {
                    _playerScore.text = ($"1. {entry.player.publicName} \t \t {entry.score}");
                }
            });
        }
#if !UNITY_WEBGL || UNITY_EDITOR
#endif
    }
}
