using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class ButtonsAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform[] _rectTransforms;
    [SerializeField] private CanvasGroup _canvasGroup;

    private const float Duration = 1f;

    public void SetFadeDown()
    {
        Debug.Log(_canvasGroup);

        var tween = _canvasGroup.DOFade(0, Duration);
        tween.OnComplete(DisableCanvas);
    }

    private void OnEnable()
    {
        if (_rectTransforms == null)
            throw new InvalidOperationException();

        Move();
    }

    private void Move()
    {
        foreach (var rect in _rectTransforms)
        {
            rect.DOAnchorPos(Vector3.zero, Duration).SetEase(Ease.InBack);
        }
    }

    private void DisableCanvas()
    {
        _canvasGroup.gameObject.SetActive(false);
    }
}
