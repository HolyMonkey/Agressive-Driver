using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private float _slowdownFactor;
    [SerializeField] private float _slowdownLength;
    [SerializeField] private float _chancePercentage;

    private float _chance;
    private float _timeScale;

    private void OnEnable()
    {
        _player.PlayerHited += OnPlayerHited;
        _playerChanger.PlayerChanged += OnPlayerChanged;
    }

    private void OnDisable()
    {
        _player.PlayerHited -= OnPlayerHited;
        _playerChanger.PlayerChanged -= OnPlayerChanged;
    }

    private void Awake()
    {
        _chance = _chancePercentage / 100f;
    }

    private void OnPlayerChanged(Player newPlayer)
    {
        _player = newPlayer;
    }

    private void OnPlayerHited(Vector3 hitPoint)
    {
        if(Random.value <= _chance)
            RunSlowMotion();
    }

    private void RunSlowMotion()
    {
        _timeScale = _slowdownFactor;
        Time.fixedDeltaTime = _timeScale * 0.02f;
    }
}