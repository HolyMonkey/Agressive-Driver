using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Agava.YandexGames;
using DeviceType = Agava.YandexGames.DeviceType;

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
    [SerializeField] private GameObject _phoneButtons;

    private StringBuilder _x = new StringBuilder("x ");
    private bool _isFinished = false;
    private int _allScore = 0;
    private int _nextLevelIndex;

    public bool IsFinished => _isFinished;
    
    public event Action Finished;
    public event Action<int,int> PointsCounted;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Player player) && _isFinished == false)
        {
            if (player.GetComponent<PlayerMover>().IsPlayerDied == false)
            {
                if (Device.Type == DeviceType.Mobile)
                    _phoneButtons.SetActive(false);
                
                _ad.EditIsTapedReviveButtonToFalse();
                _camera.GetComponent<CameraFollow>().enabled = false;
                player.enabled = false;
                _WinPanel.SetActive(true);
                int near = _points.NearMissCount * 100;
                int board = _points.BoardingCount * 50;
                int score = near + board;
                _boardingCount.text = _x + _points.BoardingCount.ToString();
                _nearMissCount.text = _x + _points.NearMissCount.ToString();
                _boardingTotal.text = board.ToString();
                _nearMissTotal.text = near.ToString();
                _Total.text = (near + board).ToString();
                _TotalAll.text = score.ToString();
                _isFinished = true;
                _allScore += score;
                Finished?.Invoke();
                ChangeNextLevelIndex();
                PointsCounted?.Invoke(_allScore,_nextLevelIndex);
                ChangeNextLevelIndex();
            }
        }
    }

    private void ChangeNextLevelIndex()
    {
        _nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (_nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
            _nextLevelIndex = 0;
    }

    private void OnClick()
    {
        SceneManager.LoadScene(_nextLevelIndex);
    }
}
