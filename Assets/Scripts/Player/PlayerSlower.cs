using System;
using UnityEngine;

public class PlayerSlower : MonoBehaviour
{
    [SerializeField] private PlayerMover _playerMover;

    [HideInInspector] public bool _isPressed = false;
    
    private float _pressTime;
    private float _pressTimeNeed = 0.25f;
    
    private float _minSpeed = 20f;

    public void SlowPlayerSpeed(bool pressed)
    {
        // Vector3 velocity = _playerMover.PlayerRigidbody.velocity;
        // float nextSpeed = velocity.magnitude - 1f;
        // Mathf.Clamp(nextSpeed, _minSpeed, velocity.magnitude);
        // _playerMover.PlayerRigidbody.velocity = new Vector3(velocity.x, velocity.y, velocity.z - 1f);
        //_playerMover.PressingBrake(true);
        _isPressed = pressed;
        //_playerMover.PressingBrake(_pressTime > _pressTimeNeed);
    }

    private void Update()
    {
        // if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        // {
        //     _pressTime = 0;
        // }
        //
        // if (Input.GetMouseButton(0))
        // {
        //     _pressTime += Time.deltaTime;
        // }
        // _playerMover.PressingBrake(_pressTime > _pressTimeNeed);
        
        _playerMover.PressingBrake(_isPressed);
        //_isPressed = false;
    }
}