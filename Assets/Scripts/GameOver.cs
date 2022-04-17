using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DeviceType = Agava.YandexGames.DeviceType;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private FinishLevel _finishLevel;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _phoneButtons;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _reviveButton;

    private Advertisement _ad;
    private bool _isGameOver = false;

    public bool IsGameOver => _isGameOver;

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
        if (_finishLevel.IsFinished == false)
        {
            if (Device.Type == DeviceType.Mobile)
                _phoneButtons.SetActive(false);

            _gameOverPanel.SetActive(true);

            if (_ad.IsTapedReviveButton)
            {
                _reviveButton.gameObject.SetActive(false);
            }

            _isGameOver = true;
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetActiveGameOverPanel()
    {
        _gameOverPanel.SetActive(false);
        _isGameOver = false;
        
      if(Device.Type == DeviceType.Mobile)
            _phoneButtons.SetActive(true);
    }
}
