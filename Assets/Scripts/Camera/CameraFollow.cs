using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Player _target;
    [SerializeField] private float _smoothSpeed;

    private Vector3 _offset;
    private Vector3 _targetPosition;

    private void Start()
    {
        _offset = transform.position - _target.transform.position;
    }

    private void FixedUpdate()
    {
        _targetPosition = _target.transform.position + _offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, _targetPosition, _smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }
}
