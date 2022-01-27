using System;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private TargetPoint _targetPoint;
    [SerializeField] private PlayerSlower _playerSlower;
    [SerializeField] private PlayerChanger _playerChanger;
    
    public event Action<float> OnSwipe;

    private const float DeadZone = 0.3f;
    private Vector2 _tapPosition;
    private Vector2 _swipeDelta;
    private bool _isSwipe;
    private Vector2 _zero;

    private float _pressTime;
    private float _pressTimeNeed = 0.1f;

    private TouchPhase _beganTouchPhase;
    private TouchPhase _stationaryTouchPhase;
    private TouchPhase _endedTouchPhase;

    private void Start()
    {
        _zero = Vector2.zero;

        _beganTouchPhase = TouchPhase.Began;
        _stationaryTouchPhase = TouchPhase.Stationary;
        _endedTouchPhase = TouchPhase.Ended;
    }

    private void Update()
    {
        //if (Input.touchCount > 0)
        //{
        //    var touch = Input.GetTouch(0);
        //    if (touch.phase == _beganTouchPhase || touch.phase == _stationaryTouchPhase)
        //    {
        //        _isSwipe = true;
        //        _pressTime += Time.deltaTime;
        //        _tapPosition = Input.GetTouch(0).position;
        //    }
        //    else if (touch.phase == _endedTouchPhase)
        //    {
        //        _pressTime = 0;
        //        ResetSwap();
        //    }
        //}
        //CheckSwipe(); 
    }

    private void OnEnable()
    {
        _playerChanger.PlayerSlowerChanged += OnPlayerSlowerChanged;
    }

    private void OnDisable()
    {
        _playerChanger.PlayerSlowerChanged -= OnPlayerSlowerChanged;
    }

    private void OnPlayerSlowerChanged(PlayerSlower newPlayerSlower)
    {
        _playerSlower = newPlayerSlower;
    }

    private void CheckSwipe()
    {
        _swipeDelta = _zero;
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
        _tapPosition = _zero;
        _swipeDelta = _zero;
        _playerSlower.SlowPlayerSpeed(false);
    }
}