using UnityEngine;
using YandexGames;
using TMPro;
using System.Collections;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerScore;
    [SerializeField] private GameObject _leaderBoardPanel;

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
        _leaderBoardPanel.SetActive(false);

        if (PlayerPrefs.HasKey("AllScore"))
        {
            int score = PlayerPrefs.GetInt("AllScore");

            Leaderboard.SetScore("PlaytestBoard", score);
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif

        // Always wait for it if invoking something immediately in the first scene.
        yield return YandexGamesSdk.WaitForInitialization();
    }

    public void GetLeaderBoard()
    {
        _leaderBoardPanel.SetActive(true);

        Leaderboard.GetEntries("PlaytestBoard", (result) =>
        {
            var entries = result.entries;

            foreach (var entry in entries)
            {
                string name = entry.player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = "Anonymous";

                int score = entry.score;
                string playerScore = $"{entry.rank} \t\t {name} \t\t {score}\n";

                _playerScore.text += playerScore;
            }
        });
    }

    public void Close()
    {
        _playerScore.text = string.Empty;
        _leaderBoardPanel.SetActive(false);
    }
}
