using UnityEngine;
using DG.Tweening;

public class StartGameHider : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private const float Duration = 0.5f;

    public void Show()
    {
        _canvasGroup.DOFade(1, Duration); 
    }

    public void Hide()
    {
        var tweenFade = _canvasGroup.DOFade(0, Duration);
        SetOffCanvasGroup();
    }

    private void SetOffCanvasGroup()
    {
        _canvasGroup.interactable = false;
    }
}
