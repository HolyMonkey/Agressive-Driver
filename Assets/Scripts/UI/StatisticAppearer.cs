using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StatisticAppearer : MonoBehaviour
{
    [SerializeField] private FinishLevel _finish;
    [SerializeField] private List<CanvasGroup> _statisticTexts;
    [SerializeField] private float _duration;

    private const int EndValue = 1;
    private Queue<CanvasGroup> _canvasGroups;

    private void OnEnable()
    {
        _finish.Finished += OnLevelFinished;
    }

    private void Start()
    {
        foreach (var statistic in _statisticTexts)
        {
            statistic.alpha = 0;
        }

        _canvasGroups = new Queue<CanvasGroup>(_statisticTexts);
    }

    private void OnDisable()
    {
        _finish.Finished -= OnLevelFinished;
    }

    private void OnLevelFinished()
    {
        PeekNextGroup();
    }

    private void PeekNextGroup()
    {
        if(_canvasGroups.Count == 0)
            return;
        var group = _canvasGroups.Peek();
        StartCoroutine(FadeGroup(group));
        _canvasGroups.Dequeue();
    }
    
    private IEnumerator FadeGroup(CanvasGroup group)
    {
        group.DOFade(EndValue, _duration);
        yield return new WaitForSeconds(_duration);
        PeekNextGroup();
    }
}