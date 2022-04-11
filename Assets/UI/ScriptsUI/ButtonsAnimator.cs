using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class ButtonsAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform[] _rectTransforms;
    [SerializeField] private PlayerSelector _playerSelector;
    [SerializeField] private CanvasGroup _playSelector;
    [SerializeField] private CanvasGroup _tapToStart;

    private const float Duration = 0.5f;
    private const float ScaleDown = 0.8f;

    public void DisablePlayerSelector()
    {
        var tween = _playSelector.DOFade(0, Duration);
        tween.OnComplete(DisableCanvas);
    }

    public void DisableStart()
    {
        _tapToStart.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (_rectTransforms == null || _playerSelector == null || _playSelector == null || _tapToStart == null)
            throw new InvalidOperationException(nameof(ButtonsAnimator));

        _playerSelector.ButtonSelected += SetSizeDown;
        _tapToStart.alpha = 0;

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
        ShowStart();
        _playSelector.gameObject.SetActive(false);
    }

    private void ShowStart()
    {
        var tween = _tapToStart.DOFade(1, Duration);
        EnableStart();
    }

    private void EnableStart()
    {
        _tapToStart.interactable = true;
    }
}
