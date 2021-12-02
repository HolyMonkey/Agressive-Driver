using UnityEngine;

public class StartGameHider : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    public void Hide()
    {
        _canvasGroup.alpha = 0;
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
    }

    public void SetOffCanvasGroup()
    {
        _canvasGroup.gameObject.SetActive(false);
    }
}
