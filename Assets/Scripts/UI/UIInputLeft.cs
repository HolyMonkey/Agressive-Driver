using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInputLeft : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerInput _playerInput;
    private bool _isPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
    }

    private void Update()
    {
        //WW_playerInput.SetPressed(_isPressed);
    }
}
