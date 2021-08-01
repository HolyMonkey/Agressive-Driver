using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Player _target;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private float _smoothSpeed;
    [SerializeField] private Rigidbody rbody;

    private Vector3 _offset;
    private Vector3 _targetPosition;

    private void Start()
    {
        _offset = transform.position - _target.transform.position;
    }

    private void OnEnable()
    {
        _playerChanger.PlayerChanged += OnPlayerChange;
    }

    private void OnDisable()
    {
        _playerChanger.PlayerChanged -= OnPlayerChange;
    }

    private void FixedUpdate()
    {
        _targetPosition = _target.transform.position + _offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, _targetPosition, _smoothSpeed * Time.fixedDeltaTime);

        transform.position = smoothedPosition;

        transform.LookAt(_target.transform);
    }

    private void OnPlayerChange(Player newPlayer)
    {
        _target = newPlayer;
    }
}
