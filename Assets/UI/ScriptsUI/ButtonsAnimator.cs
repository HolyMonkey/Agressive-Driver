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

    public void SetFadeDown()
    {
        Debug.Log(_canvasGroup);

        var tween = _canvasGroup.DOFade(0, Duration);
        tween.OnComplete(DisableCanvas);
    }

    private void OnEnable()
    {
        if (_rectTransforms == null || _playerSelector == null || _canvasGroup == null)
            throw new InvalidOperationException();

        _playerSelector.ButtonSelected += SetSize;

        Move();
    }

    private void OnDisable()
    {
        _playerSelector.ButtonSelected -= SetSize;
    }

    private void Move()
    {
        foreach (var rect in _rectTransforms)
        {
            rect.DOAnchorPos(Vector3.zero, Duration).SetEase(Ease.InBack);
        }
    }

    private void SetSize(Button button)
    {

    }

    private void DisableCanvas()
    {
        _canvasGroup.gameObject.SetActive(false);
    }
}
