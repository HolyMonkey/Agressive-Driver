using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyRoads3Dv3;

public class TargetPoint : MonoBehaviour
{
    [SerializeField] private Transform _wayDirection;
    [SerializeField] private PlayerMover _vehicleControl;
    [SerializeField] private float _maxRightTurn;
    [SerializeField] private float _maxLeftTurn;
    [SerializeField] private float _stepSize;
    [SerializeField] private float _lengthFromVehicle;

    private Vector3 _targetPosition;
        
    [SerializeField] private ERModularRoad eRoad;

    private Vector3 waypointStart;
    private Vector3 waypointEnd;
    private Vector3 waypointDirection;
    private Vector3 waypointRightDirection;
    private int waypointIndex = 2;
    private float lastDistance;

    private void Start()
    {
        transform.position = _vehicleControl.transform.position;

        _targetPosition = transform.position;
        SetWaypoints(waypointIndex);
    }

    private void SetWaypoints(int index)
    {
        waypointStart = eRoad.soSplinePoints[waypointIndex];
        waypointIndex++;
        if (waypointIndex == eRoad.soSplinePoints.Count)
        {
            enabled = false;
            return;
        }
        waypointEnd = eRoad.soSplinePoints[waypointIndex];
        
        var dir = (waypointEnd - waypointStart).normalized;
        waypointDirection = dir;
        waypointRightDirection = Quaternion.AngleAxis(90, Vector3.up) * dir;

        lastDistance = Vector3.Distance(_vehicleControl.transform.position, waypointEnd);
    }

    private void CheckWaypointDestination()
    {
        var distance = Vector3.Distance(_vehicleControl.transform.position, waypointEnd);
        if (distance > lastDistance)
        {            
            SetWaypoints(waypointIndex);
        }
        else
            lastDistance = distance;
    }

    private void Update()
    {
        CheckWaypointDestination();

        float length = Vector3.Distance(_vehicleControl.transform.position, waypointStart) + _lengthFromVehicle;
        transform.position = waypointStart + waypointDirection * length;
    }

    public void TryTurnRight()
    {
        //if (_targetPosition.x < _maxRightTurn)
        { //_stepSize
            SetNextPosition(_stepSize);
        }
    }

    public void TryTurnLeft()
    {
        //if (_targetPosition.x > _maxLeftTurn)
        {
            SetNextPosition(-_stepSize);
        }
    }


    private void SetNextPosition(float stepSize)
    {
        float length = Vector3.Distance(_vehicleControl.transform.position, waypointStart) + _lengthFromVehicle;
        transform.position = waypointStart + waypointDirection * length;

        transform.position = waypointStart + waypointDirection * length + waypointRightDirection * stepSize;
        //_targetPosition = new Vector3(_targetPosition.x + stepSize, transform.position.y, transform.position.z);
    }

    public void SetWayDirection(Transform wayDirection)
    {
        _wayDirection = wayDirection;
    }
}
