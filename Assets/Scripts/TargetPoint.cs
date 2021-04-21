using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyRoads3Dv3;

public class TargetPoint : MonoBehaviour
{
    [SerializeField] private SwipeDetector _swipeDetector;
    [SerializeField] private PlayerMover _vehicleControl;
    [SerializeField] private Player _player;
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

    private float timeToLoose = 5;

    private bool _isRightLine = true;

    private void Start()
    {
        transform.position = _vehicleControl.transform.position;
        _targetPosition = transform.position;
        TryTurnRight();
    }

    private void OnEnable()
    {
        _swipeDetector.OnSwipe += OnSwipe;
    }

    private void OnDisable()
    {
        _swipeDetector.OnSwipe -= OnSwipe;
    }

    private void OnSwipe(SwipeData data)
    {
        if (data.Direction == SwipeDirection.Left)
        {
            _isRightLine = false;
        }
        if (data.Direction == SwipeDirection.Right)
        {
            _isRightLine = true;
        }
    }
    
    private void SetWaypoints()
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

        timeToLoose = 5;
    }

    private void CheckWaypointDestination()
    {
       // var distance = Vector3.Distance(_vehicleControl.transform.position, waypointEnd);
        //if (distance > lastDistance)
        if (_vehicleControl.transform.position.z > waypointEnd.z)
        {
            SetWaypoints();
        }
        else
        {
            
            timeToLoose -= Time.deltaTime;
            if (timeToLoose < 0)
                _player.TakeDamage(5000);
                
            //lastDistance = distance;
        }
    }

    
    private void Update()
    {
        CheckWaypointDestination();

        //if (lastDistance > 100)
        //    _player.TakeDamage(5000);

        float length = Vector3.Distance(_vehicleControl.transform.position, waypointStart) + _lengthFromVehicle;
        transform.position = waypointStart + waypointDirection * length;

        if (_isRightLine)
        {
            TryTurnRight();
        }
        else
        {
            TryTurnLeft();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _isRightLine = false;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _isRightLine = true;
        }
    }

    public void TryTurnRight()
    {
        SetNextPosition(_stepSize);
    }

    public void TryTurnLeft()
    {
        SetNextPosition(-_stepSize);        
    }


    private void SetNextPosition(float stepSize)
    {
        float length = Vector3.Distance(_vehicleControl.transform.position, waypointStart) + _lengthFromVehicle;
        transform.position = waypointStart + waypointDirection * length;

        transform.position = waypointStart + waypointDirection * length + waypointRightDirection * stepSize;
    }

}
