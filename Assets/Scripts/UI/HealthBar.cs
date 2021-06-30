using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player _health;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _duration;

    private Coroutine _changing;

    private void OnEnable()
    {
        _health.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int currentValue, int maxValue)
    {
        float nextValue = (float) currentValue / maxValue;
        if (_changing == null)
        {
            _changing = StartCoroutine(SliderValueChange(_slider.value, nextValue, _duration));
        }
        else
        {
            StopCoroutine(_changing);
            _changing = StartCoroutine(SliderValueChange(_slider.value, nextValue, _duration));
        }
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