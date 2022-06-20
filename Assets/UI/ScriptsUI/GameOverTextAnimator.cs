using UnityEngine;
using DG.Tweening;
using System;

public class GameOverTextAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform[] _texts;
    [SerializeField] private RectTransform _crashText;

    private CanvasGroup _canvasGroup;

    private const float Duration = 0.8f;
    private const float Spliter = 20f;

    private void OnEnable()
    {
        if (_crashText == null || _texts == null)
            throw new ArgumentNullException()
                ;
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
        var tween = _crashText.DOScale(1, Duration / 2);
    }
}