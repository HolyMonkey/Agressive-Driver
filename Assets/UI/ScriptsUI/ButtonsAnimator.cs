using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class ButtonsAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform[] _rectTransforms;
    [SerializeField] private PlayerSelector _playerSelector;
    [SerializeField] private CanvasGroup _canvasGroup;

    private const float Duration = 1f;
    private const float ScaleDown = 0.8f;

    public void SetFadeDown()
    {
        var tween = _canvasGroup.DOFade(0, Duration);
        tween.OnComplete(DisableCanvas);
    }

    private void OnEnable()
    {
        if (_rectTransforms == null || _playerSelector == null || _canvasGroup == null)
            throw new InvalidOperationException();

        _playerSelector.ButtonSelected += SetSizeDown;

        Move();
    }

    private void OnDisable()
    {
        _playerSelector.ButtonSelected -= SetSizeDown;
    }

    private void Move()
    {
        foreach (var rect in _rectTransforms)
        {
            rect.DOAnchorPos(Vector3.zero, Duration).SetEase(Ease.InBack);
        }
    }

    private void SetSizeDown(Button button)
    {
        float delay = 0.2f;
        var tween = button.transform.DOScale(ScaleDown, delay);
        var tween2 = button.transform.DOScale(1, Duration / 2).SetDelay(delay);
    }

    private void DisableCanvas()
    {
        _canvasGroup.gameObject.SetActive(false);
    }
}
