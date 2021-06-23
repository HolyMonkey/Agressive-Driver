using UnityEngine;
using EasyRoads3Dv3;

public class TargetPoint : MonoBehaviour
{
    [SerializeField] private SwipeDetection _detection;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private Player _player;
    [SerializeField] private float _stepSize;
    [SerializeField] private float _lengthFromVehicle;
    [SerializeField] private ERModularRoad eRoad;

    private Vector3 waypointStart;
    private Vector3 waypointEnd;
    private Vector3 waypointDirection;
    private Vector3 waypointRightDirection;
    private int waypointIndex = 2;
    private float timeToLoose = 5;

    private float _startStepSize;
    private float _currentStepSize;

    private void Start()
    {
        _startStepSize = _stepSize;
        _currentStepSize = _startStepSize;

        transform.position = _playerMover.transform.position;
        SetNextPosition(_currentStepSize);
    }

    private void OnEnable()
    {
        _detection.OnSwipe += OnSwipe;
    }

    private void OnDisable()
    {
        _detection.OnSwipe -= OnSwipe;
    }

    public void OnSwipe(float delta)
    {
        delta /= 500;
        var clampedDelta = Mathf.Clamp(delta, -_stepSize, _stepSize);
        _currentStepSize += clampedDelta;
        _currentStepSize = Mathf.Clamp(_currentStepSize, -_stepSize, _stepSize);
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
        timeToLoose = 5;
    }

    private void CheckWaypointDestination()
    {
        if (_playerMover.transform.position.z > waypointEnd.z)
        {
            SetWaypoints();
        }
        else
        {
            timeToLoose -= Time.deltaTime;
            if (timeToLoose < 0)
                _player.TakeDamage(5000);
        }
    }

    private void Update()
    {
        CheckWaypointDestination();
        SetNextPosition(_currentStepSize);
        if (Input.GetKey(KeyCode.A))
        {
            _currentStepSize -= 0.05f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _currentStepSize += 0.05f;
        }
        _currentStepSize = Mathf.Clamp(_currentStepSize, -_stepSize, _stepSize);
    }
    
    private void SetNextPosition(float stepSize)
    {
        float length = Vector3.Distance(_playerMover.transform.position, waypointStart) + _lengthFromVehicle;
        transform.position = waypointStart + waypointDirection * length + waypointRightDirection * stepSize;
    }
}