using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private float _slowdownFactor;
    [SerializeField] private float _slowdownLength;
    [SerializeField] private float _chancePercentage;

    private float _chance;

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

    private void Update()
    {
        Time.timeScale += (1f / _slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
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
        Time.timeScale = _slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}