using UnityEngine;
using Agava.YandexGames;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerScore;
    [SerializeField] private GameObject _leaderBoardPanel;
    [SerializeField] private string _name = "PlaytestBoard";
    [SerializeField] private EntryView _playerEntryView;
    [SerializeField] private EntryViewPool _playerEntriesViewPool;

    private string _playerName;
    private List<EntryView> _entryViews = new List<EntryView>();

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

    public void Show()
    {
        _leaderBoardPanel.SetActive(true);
            
        Leaderboard.GetEntries(_name, (result) =>
        {
            foreach (var entryView in _entryViews)
                entryView.gameObject.SetActive(false);

            _entryViews.Clear();
            Leaderboard.GetPlayerEntry(_name, (playerEntry) =>
            {
                _playerEntryView.Init(playerEntry.rank.ToString(), playerEntry.player.publicName, playerEntry.score.ToString());
            });

            foreach (var entry in result.entries)
            {
                _playerName = entry.player.publicName;                                                                
                EntryView entryView = _playerEntriesViewPool.GetFreeObject();
                entryView.Init(entry.rank.ToString(), _playerName, entry.score.ToString());
                entryView.gameObject.SetActive(true);
                _entryViews.Add(entryView);
            }
        });
        /*
       foreach (var entryView in _entryViews)
           entryView.gameObject.SetActive(false);
       
       _entryViews.Clear();
       
       _playerEntryView.Init("123", "123","123");
       
       float _rankk = 1;
       foreach (var entry in _test)
       {
           _playerName = "123";                                                                
           EntryView entryView = _playerEntriesViewPool.GetFreeObject();
           entryView.Init("123", "123","123");
           entryView.gameObject.SetActive(true);
           _entryViews.Add(entryView);
       }

       _playerEntryView.Init("321", "1eqweqe3", "1e12dasd23");
       */
    }

    public void Close()
    {      
        _leaderBoardPanel.SetActive(false);
    }
}
