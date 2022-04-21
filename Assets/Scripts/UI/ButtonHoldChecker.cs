using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoldChecker : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private bool _isPressed = false;

    public bool IsPressed => _isPressed;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
    } 
}
