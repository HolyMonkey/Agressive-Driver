using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Button _restartButton;
    private bool _isGameOver = false;

    private void OnEnable()
    {
        _player.PlayerDied += StartGameOver;
        _player.Revived += StopGameOver;
        _restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDisable()
    {
        _player.PlayerDied -= StartGameOver;
        _player.Revived -= StopGameOver;
        _restartButton.onClick.RemoveListener(RestartGame);
    }

    private void StopGameOver()
    {
        _gameOverPanel.SetActive(false);
        _isGameOver = false;
    }
    
    private void StartGameOver()
    {
        StartCoroutine(ShowGameOver());
    }

    private IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        _gameOverPanel.SetActive(true);
        _isGameOver = true;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}