using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TotalScoreChanger : MonoBehaviour
{
    [SerializeField] private UIPointMover _uiPointMover;
    [SerializeField] private float _duration;
    
    private int _totalScore;
    private TMP_Text _scoreText;
    
    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _uiPointMover.PointsReached += OnPointsReached;
    }

    private void OnDisable()
    {
        _uiPointMover.PointsReached -= OnPointsReached;
    }

    private void OnPointsReached(int score)
    {
        _totalScore += score;
        _scoreText.DOText(_totalScore.ToString(), _duration, true, ScrambleMode.Numerals);
    }
}