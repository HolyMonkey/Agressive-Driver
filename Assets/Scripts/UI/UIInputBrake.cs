using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInputBrake : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerMover _playerMover;

    private bool _isPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
    }

    public void Update()
    {
        _playerMover.PressingBrake(_isPressed);
    }

}
