using System;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private TargetPoint _targetPoint;
    
    public event Action<float> OnSwipe;

    private const float DeadZone = 0f;
    private Vector2 _tapPosition;
    private Vector2 _swipeDelta;
    private bool _isSwipe;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _isSwipe = true;
                _tapPosition = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended ||
                     Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                ResetSwap();
            }
        }
        CheckSwipe();
        
        // if (Input.GetMouseButtonDown(0))
        // {
        //     _isSwipe = true;
        //     _tapPosition = Input.mousePosition;
        // }
        // else if (Input.GetMouseButtonUp(0))
        // {
        //     ResetSwap();
        // } 
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
            // else if (Input.GetMouseButtonDown(0))
            // {
            //     _swipeDelta = (Vector2)Input.mousePosition -_tapPosition;
            // }
        }
        if (_swipeDelta.magnitude > DeadZone)
        {
            // OnSwipe?.Invoke(_swipeDelta.x);
            _targetPoint.OnSwipe(_swipeDelta.x);
        }
    }

    private void ResetSwap()
    {
        _isSwipe = false;
        _tapPosition = Vector2.zero;
        _swipeDelta = Vector2.zero;
    }
}