using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class FinishTextAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform[] _texts;
    [SerializeField] private RectTransform _finishText;

    private CanvasGroup _canvasGroup;

    private const float Duration = 1f;
    private const float Spliter = 20f;

    private void OnEnable()
    {
        if (_finishText == null || _texts == null)
            throw new InvalidOperationException();

        _canvasGroup = GetComponent<CanvasGroup>();

        EnablePanel();
    }

    private void EnablePanel()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        var tween = _canvasGroup.DOFade(1, Duration / 2);
        tween.OnComplete(Move);
    }

    private void Move()
    {
        SetSize();

        for (int i = 0; i < _texts.Length; i++)
        {
            _texts[i].DOAnchorPos(Vector3.zero, Duration).SetDelay(i / Spliter);
        }
    }

    private void SetSize()
    {
        var tween = _finishText.DOScale(1, Duration);
    }
}
