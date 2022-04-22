using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]

public class ButtonHoldChecker : MonoBehaviour
{
    private bool _isPressed;
    private bool _checkForPressed;
    private bool _checkForSetPressedFalse;
    private RectTransform _rectTransform;

    public bool IsPressed => _isPressed;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _isPressed = false;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            _checkForPressed = false;
            foreach (var touch in Input.touches)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, touch.position, null) &&
                    _isPressed == false)
                {
                    _isPressed = true;
                    _checkForPressed = true;
                }
            }

            if (_checkForPressed == false)
                Invoke(nameof(SetIsPressedFalse), 0.1f);
        }
        else
        {
            Invoke(nameof(SetIsPressedFalse), 0.1f);
        }
    }

    private void SetIsPressedFalse()
    {
        _checkForSetPressedFalse = true;
        
        foreach (var touch in Input.touches)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, touch.position, null))
            {
                _checkForSetPressedFalse = false;
            }
        }

        if (_checkForSetPressedFalse)
            _isPressed = false;
    }
}