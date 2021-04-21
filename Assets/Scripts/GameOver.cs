using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _gameOverPanel;
    private bool _isGameOver = false;

    private void OnEnable()
    {
        _player.PlayerDied += StartGameOver;
    }

    private void OnDisable()
    {
        _player.PlayerDied -= StartGameOver;
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

    private void Update()
    {
        if (_isGameOver && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
