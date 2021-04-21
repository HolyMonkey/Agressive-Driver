using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool isFinished = false;

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
            _boardingCount.text = "x "+_points.BoardingCount.ToString();
            _nearMissCount.text = "x " + _points.NearMissCount.ToString();
            _boardingTotal.text = "" + (board).ToString();
            _nearMissTotal.text = "" + (near).ToString();
            _Total.text = (near + board).ToString();
            _TotalAll.text = (score).ToString();
            isFinished = true;
        }
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

    private void Update()
    {
        if (isFinished && Input.GetMouseButtonDown(0))
        {
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
                nextLevelIndex = 0;
            SceneManager.LoadScene(nextLevelIndex);
        }
    }
}
