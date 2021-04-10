using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMover))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private TargetPoint _targetPoint;
    [SerializeField] private float _doubleClickTime;
    
    private PlayerMover _playerMover;
    private float _lastClickTime;

    private void Start()
    {
        _playerMover = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _targetPoint.TryTurnLeft();
        }
        else
        {
            _targetPoint.TryTurnRight();
            _playerMover.SetPower(false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            float _timeSinceLastClick = Time.time - _lastClickTime;

            if (_timeSinceLastClick <= _doubleClickTime)
            {
                _playerMover.SetPower(true);
            }

            _lastClickTime = Time.time;
        }
    }
}
