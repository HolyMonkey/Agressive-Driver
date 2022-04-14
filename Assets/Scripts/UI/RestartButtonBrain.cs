using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButtonBrain : MonoBehaviour
{
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private UIStartGame _uiStartGame;
    [SerializeField] private Button _restartButton;
    [SerializeField] private float _amountSecondsForPlayerStuckCheck = 2.5f;
    [SerializeField] private GameOver _gameOver;
    [SerializeField] private FinishLevel _finishLevel;
    
    private WaitForSeconds _waitForSeconds;
    private PlayerMover _playerMover;
    private bool _gameStarted;
    
    private void OnEnable()
    {
        _playerChanger.PlayerMoverChanged += OnPlayerMoverChanged;
        _uiStartGame.GameStarted += OnGameStarted;
        _restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDisable()
    {
        _playerChanger.PlayerMoverChanged -= OnPlayerMoverChanged;
        _uiStartGame.GameStarted -= OnGameStarted;
        _restartButton.onClick.AddListener(RestartGame);
    }

    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(_amountSecondsForPlayerStuckCheck);
    }

    private void FixedUpdate()
    {
        if (_gameStarted)
        {
            StartCoroutine(CheckForPlayerGettingStuck());
        }
    }

    private void OnPlayerMoverChanged(PlayerMover playerMover)
    {
        _playerMover = playerMover;
    }

    private void OnGameStarted()
    {
        _gameStarted = true;
    }

    private IEnumerator CheckForPlayerGettingStuck()
    {
        yield return _waitForSeconds;
        
        if (_playerMover.Speed < 1 && _gameOver.IsGameOver == false && _finishLevel.IsFinished == false)
            _restartButton.gameObject.SetActive(true);
        else if(_restartButton.isActiveAndEnabled)
            _restartButton.gameObject.SetActive(false);
    }
    
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
