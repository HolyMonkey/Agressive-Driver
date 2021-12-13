using UnityEngine;
using YandexGames;
using System.Linq;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerScore;
    [SerializeField] private GameObject _leaderBoardPanel;

    private void Start()
    {
        _leaderBoardPanel.SetActive(false);
    }

    public void GetLeaderBoard()
    {
        _leaderBoardPanel.SetActive(true);

        Leaderboard.GetEntries("TestLeaderBoard", (result) =>
        {
            var entries = result.entries;

            foreach (var entry in entries)
            {
                string name = entry.player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = "Anonymous";

                int score = entry.score;
                string playerScore = $"{entry.rank} \t\t {name} \t\t {score}";

                _playerScore.text = playerScore;
            }
        });
    }

    public void Close()
    {
        _leaderBoardPanel.SetActive(false);
    }
}
