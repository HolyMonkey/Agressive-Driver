using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIPointMover : MonoBehaviour
{
    [SerializeField] private RectTransform _endPosition;
    [SerializeField] private Points _points;
    [SerializeField] private TMP_Text _pointsText;
    [SerializeField] private float _duration;

    public event Action<int> PointsReached;
    
    private void OnEnable()
    {
        _pointsText.alpha = 0;
        _points.Collected += OnPointCollected;
    }

    private void OnDisable()
    {
        _points.Collected -= OnPointCollected;
    }

    private void OnPointCollected(int points)
    {
        var tweener = transform.DOMove(_endPosition.position, _duration);
        _pointsText.DOFade(0, _duration);
        tweener.OnComplete(OnTweenerComplete(points));
    }

    private TweenCallback OnTweenerComplete(int points)
    {
        PointsReached?.Invoke(points);
        _pointsText.alpha = 1;
        return default;
    }
}