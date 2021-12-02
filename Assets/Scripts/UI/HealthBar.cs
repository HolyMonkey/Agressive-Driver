using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _duration;

    private Coroutine _changing;

    private void OnEnable()
    {
        _player.HealthChanged += OnHealthChanged;
        _playerChanger.PlayerChanged += OnPlayerChanged;
    }

    private void OnDisable()
    {
        _player.HealthChanged -= OnHealthChanged;
        _playerChanger.PlayerChanged -= OnPlayerChanged;
    }

    private void OnPlayerChanged(Player newPlayer)
    {
        _player.HealthChanged -= OnHealthChanged;
        _player = newPlayer;
        _player.HealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(int currentValue, int maxValue)
    {
        float nextValue = (float) currentValue / maxValue;

        if (_changing != null)
            StopCoroutine(_changing);        
        _changing = StartCoroutine(SliderValueChange(_slider.value, nextValue, _duration));
    }

    private IEnumerator SliderValueChange(float startValue, float endValue, float duration)
    {
        float elapsed = 0;
        while (_slider.value != endValue)
        {
            float nextValue = Mathf.Lerp(startValue, endValue, elapsed / duration);
            _slider.value = nextValue;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}