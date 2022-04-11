using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoldChecker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isPressed = false;
    
    public bool IsPressed => _isPressed;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        _isPressed = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        _isPressed = false;
    }
}
