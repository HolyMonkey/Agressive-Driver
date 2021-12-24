using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YandexGames;

public class FinishLevel : MonoBehaviour
{
    private const string _scoreConst = "Score";

    [SerializeField] private GameObject _WinPanel;
    [SerializeField] private Camera _camera;
    [SerializeField] private Points _points;
    [SerializeField] private TextMeshProUGUI _boardingCount;
    [SerializeField] private TextMeshProUGUI _nearMissCount;
    [SerializeField] private TextMeshProUGUI _boardingTotal;
    [SerializeField] private TextMeshProUGUI _nearMissTotal;
    [SerializeField] private TextMeshProUGUI _Total;
    [SerializeField] private TextMeshProUGUI _TotalAll;
    [SerializeField] private Button _button;
    [SerializeField] private Advertisement _ad;

    private StringBuilder _x = new StringBuilder("x ");
    private bool isFinished = false;
    private int _allScore = 0;

    public event Action Finished;

    private void Start()
    {
        if (PlayerPrefs.HasKey("AllScore"))
        {
            _allScore = PlayerPrefs.GetInt("AllScore");
            Leaderboard.SetScore("PlaytestBoard", _allScore);
        }
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _ad.EditIsTapedReviveButtonToFalse();
            _camera.GetComponent<CameraFollow>().enabled = false;
            player.enabled = false;
            _WinPanel.SetActive(true);
            int near = _points.NearMissCount * 100;
            int board = _points.BoardingCount * 50;
            int score = near + board + LoadScore();
            SaveScore(score);
            _boardingCount.text = _x + _points.BoardingCount.ToString();
            _nearMissCount.text = _x + _points.NearMissCount.ToString();
            _boardingTotal.text = board.ToString();
            _nearMissTotal.text = near.ToString();
            _Total.text = (near + board).ToString();
            _TotalAll.text = score.ToString();
            isFinished = true;

            _allScore += score;

            PlayerPrefs.SetInt("AllScore", _allScore);
            Leaderboard.SetScore("PlaytestBoard", _allScore);
            Finished?.Invoke();
        }
    }
    
    private int LoadScore()
    {
        return PlayerPrefs.GetInt(_scoreConst, 0);
    }

    private void SaveScore(int score)
    {
        PlayerPrefs.SetInt(_scoreConst, score);
        PlayerPrefs.Save();
    }

    private void OnClick()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
            nextLevelIndex = 0;
        SceneManager.LoadScene(nextLevelIndex);
    }
}
