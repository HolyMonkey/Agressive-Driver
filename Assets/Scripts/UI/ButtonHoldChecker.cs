using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoldChecker : MonoBehaviour
{
    private bool _isPressed = false;
    
    public bool IsPressed => _isPressed;
    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPressed = true;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        _isPressed = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
    }
    */
    
    public void SetIsPressed()
    {
    _isPressed = true;
    }
    
    public void SetIsPressedFalse()
    {
    _isPressed = false;
    }
}
