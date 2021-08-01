using System.Collections;
using System.Collections.Generic;
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
}
