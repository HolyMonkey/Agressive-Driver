using System;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private TargetPoint _targetPoint;
    [SerializeField] private PlayerSlower _playerSlower;
    
    public event Action<float> OnSwipe;

    private const float DeadZone = 0f;
    private Vector2 _tapPosition;
    private Vector2 _swipeDelta;
    private bool _isSwipe;
    
    private float _pressTime;
    private float _pressTimeNeed = 0.1f;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _isSwipe = true;
                _tapPosition = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                ResetSwap();
            }
        }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            _pressTime = 0;
        }

        if (Input.GetMouseButton(0))
        {
            _isSwipe = true;
            _pressTime += Time.deltaTime;
        }
        CheckSwipe(); 
    }

    private void CheckSwipe()
    {
        _swipeDelta = Vector2.zero;
        if (_isSwipe)
        {
            if (Input.touchCount > 0)
            {
                _swipeDelta = Input.GetTouch(0).position - _tapPosition;
            }

            if (_pressTime >= _pressTimeNeed &&_swipeDelta.magnitude == 0)
            {
                _playerSlower.SlowPlayerSpeed(true);
            }
            else
            {
                _playerSlower.SlowPlayerSpeed(false);
            }
        }
        if (_swipeDelta.magnitude > DeadZone)
        {
            _targetPoint.OnSwipe(_swipeDelta.x);
        }
    }

    private void ResetSwap()
    {
        _isSwipe = false;
        _tapPosition = Vector2.zero;
        _swipeDelta = Vector2.zero;
        _playerSlower.SlowPlayerSpeed(false);
    }
}