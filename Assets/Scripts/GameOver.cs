using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _reviveButton;

    private Advertisement _ad;
    private bool _isGameOver = false;

    private void Start()
    {
        _ad = GetComponent<Advertisement>();
    }

    private void OnEnable()
    {
        _player.PlayerDied += StartGameOver;
        _restartButton.onClick.AddListener(RestartGame);
        _playerChanger.PlayerChanged += OnPlayerChanged;
    }

    private void OnDisable()
    {
        _player.PlayerDied -= StartGameOver;
        _restartButton.onClick.RemoveListener(RestartGame);
        _playerChanger.PlayerChanged -= OnPlayerChanged;
    }

    private void OnPlayerChanged(Player newPlayer)
    {
        _player.PlayerDied -= StartGameOver;
        _player = newPlayer;
        _player.PlayerDied += StartGameOver;
    }

    private void StopGameOver()
    {
        _gameOverPanel.SetActive(false);
        _isGameOver = false;
    }
    
    private void StartGameOver()
    {
        _gameOverPanel.SetActive(true);
        if (_ad.IsTapedReviveButton == true)
        {
            _reviveButton.gameObject.SetActive(false);
        }
        _isGameOver = true;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetActiveGameOverPanel()
    {
        _gameOverPanel.SetActive(false);
        _isGameOver = false;
    }
}