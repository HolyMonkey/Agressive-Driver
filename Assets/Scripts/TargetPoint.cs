using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    [SerializeField] private PlayerMover _vehicleControl;
    [SerializeField] private float _maxRightTurn;
    [SerializeField] private float _maxLeftTurn;
    [SerializeField] private float _stepSize;
    [SerializeField] private float _lengthFromVehicle;

    private Vector3 _targetPosition;

    private void Start()
    {
        transform.position = _vehicleControl.transform.position;

        _targetPosition = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3(_targetPosition.x, transform.position.y, _vehicleControl.transform.position.z + _lengthFromVehicle);
    }

    public void TryTurnRight()
    {
        if (_targetPosition.x < _maxRightTurn)
        {
            SetNextPosition(_stepSize);
        }
    }

    public void TryTurnLeft()
    {
        if (_targetPosition.x > _maxLeftTurn)
        {
            SetNextPosition(-_stepSize);
        }
    }


    private void SetNextPosition(float stepSize)
    {
        _targetPosition = new Vector3(_targetPosition.x + stepSize, transform.position.y, transform.position.z);
    }

}
