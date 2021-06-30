using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class FinishLevel : MonoBehaviour
{
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
    [SerializeField] private AdSettings _adSettings;
    private bool isFinished = false;

    public event Action Finished;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
        _adSettings.AdShowned += OnAdsClosed;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
        _adSettings.AdShowned -= OnAdsClosed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _camera.GetComponent<CameraFollow>().enabled = false;
            player.enabled = false;
            _WinPanel.SetActive(true);
            int near = _points.NearMissCount * 100;
            int board = _points.BoardingCount * 50;
            int score = near + board + LoadScore();
            SaveScore(score);
            _boardingCount.text = "x " + _points.BoardingCount;
            _nearMissCount.text = "x " + _points.NearMissCount;
            _boardingTotal.text = "" + board;
            _nearMissTotal.text = "" + near;
            _Total.text = (near + board).ToString();
            _TotalAll.text = score.ToString();
            isFinished = true;
            Finished?.Invoke();
        }
    }

    public void OnAdsClosed()
    {
        int total = Convert.ToInt32(_Total.text);
        int newTotal = total * 3;
        _Total.DOText(newTotal.ToString(), 1f, true, ScrambleMode.Numerals);
        int newScore = newTotal - total + LoadScore();
        _TotalAll.DOText(newScore.ToString(), 1f,true, ScrambleMode.Numerals);
        SaveScore(newScore);
    }
    
    private int LoadScore()
    {
        return PlayerPrefs.GetInt("Score", 0);
    }

    private void SaveScore(int score)
    {
        PlayerPrefs.SetInt("Score", score);
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