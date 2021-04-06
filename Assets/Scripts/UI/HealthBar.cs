using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] protected Slider Slider;
    [SerializeField] private Player _player;
    [SerializeField] private float _changeSpeed;

    private void OnEnable()
    {
        _player.HealthChanged += OnValueChanged;
        Slider.value = 1;
    }

    private void OnDisable()
    {
        _player.HealthChanged -= OnValueChanged;
    }

    public void OnValueChanged(int value, int maxValue)
    {
        Slider.value = 0;
    }
}
