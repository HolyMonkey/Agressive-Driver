using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private FinishLevel _finish;
    [SerializeField] private UIStartGame _uiStartGame;
    [SerializeField] private Slider _progressSlider;
    
    private float distance;
    private float lastProgress = 0;

    private void Start()
    {
        distance = Vector3.Distance(_player.transform.position, _finish.transform.position);
    }

    private void Update()
    {
        var progress = (1 - Vector3.Distance(_player.transform.position, _finish.transform.position) / distance);
        if (progress > lastProgress)
            lastProgress = progress;
        _progressSlider.value = lastProgress;
    }

    private void OnEnable()
    {
        _playerChanger.PlayerChanged += OnPlayerChanged;
        _uiStartGame.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _playerChanger.PlayerChanged -= OnPlayerChanged;
        _uiStartGame.GameStarted -= OnGameStarted;
    }

    private void OnPlayerChanged(Player newPlayer)
    {
        _player = newPlayer;
    }

    private void OnGameStarted()
    {
        _progressSlider.gameObject.SetActive(true);
    }
}
