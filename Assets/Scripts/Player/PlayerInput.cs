using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMover))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private TargetPoint _targetPoint;
    [SerializeField] private float _doubleClickTime;
    
    private PlayerMover _playerMover;
    private float _pressTime;
    private float _pressTimeNeed = 0.1f;

    private void Start()
    {
        _playerMover = GetComponent<PlayerMover>();
    }

    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            _pressTime = 0;
        }

        if (Input.GetMouseButton(0))
        {
            _pressTime += Time.deltaTime;
            if (_pressTime > 5)
                _playerMover.GetComponent<Player>().TakeDamage(5000);
        }

        _playerMover.PressingBrake(_pressTime >= _pressTimeNeed);

        /*
        if (_isPressed)
        {
            _targetPoint.TryTurnLeft();
        }
        else
        {
            _targetPoint.TryTurnRight();
            _playerMover.SetPower(false);
        }
        */
        //*/
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            float _timeSinceLastClick = Time.time - _lastClickTime;

            if (_timeSinceLastClick <= _doubleClickTime)
            {
                _playerMover.SetPower(true);
            }

            _lastClickTime = Time.time;
        }
        */
    }
}
