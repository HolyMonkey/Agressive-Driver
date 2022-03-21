using UnityEngine;
using EasyRoads3Dv3;

public class TargetPoint : MonoBehaviour
{
    [SerializeField] private SwipeDetection _detection;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private float _stepSize;
    [SerializeField] private float _lengthFromVehicle;
    [SerializeField] private ERModularRoad eRoad;

    private Vector3 waypointStart;
    private Vector3 waypointEnd;
    private Vector3 waypointDirection;
    private Vector3 waypointRightDirection;
    private int waypointIndex = 2;
    private float timeToLoose = 5;
    private Transform _transform;
    private Vector3 _up;

    private float _startStepSize;
    private float _currentStepSize;
    private float _screenWidght;
    private KeyCode _a;
    private KeyCode _d;
    private KeyCode _leftArrow;
    private KeyCode _rightArrow;

    private void Start()
    {
        _a = KeyCode.A;
        _d = KeyCode.D;
        _leftArrow = KeyCode.LeftArrow;
        _rightArrow = KeyCode.RightArrow;

        _screenWidght = Screen.width;
        _up = Vector3.up;
        _transform = GetComponent<Transform>();

        _startStepSize = _stepSize;
        _currentStepSize = _startStepSize;

        transform.position = _playerMover.transform.position;
        SetNextPosition(_currentStepSize);
    }

    private void OnEnable()
    {
        _detection.OnSwipe += OnSwipe;
        _playerChanger.PlayerChanged += OnPlayerChanged;
        _playerChanger.PlayerMoverChanged += OnPlayerMoverChanged;
    }

    private void OnDisable()
    {
        _detection.OnSwipe -= OnSwipe;
        _playerChanger.PlayerChanged -= OnPlayerChanged;
        _playerChanger.PlayerMoverChanged -= OnPlayerMoverChanged;
    }

    public void OnSwipe(float delta)
    {
        delta /= 450;
        var clampedDelta = Mathf.Clamp(delta, -_stepSize, _stepSize);
        _currentStepSize += clampedDelta;
        _currentStepSize = Mathf.Clamp(_currentStepSize, -_stepSize, _stepSize);
    }


    private void OnPlayerChanged(Player newPlayer)
    {
        _player = newPlayer;
    }

    private void OnPlayerMoverChanged(PlayerMover newPlayerMover)
    {
        _playerMover = newPlayerMover;
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
        waypointRightDirection = Quaternion.AngleAxis(90, _up) * dir;
        timeToLoose = 5;
    }

    private void CheckWaypointDestination()
    {
        if (_playerMover.transform.position.z > waypointEnd.z)
        {
            SetWaypoints();
        }
    }

    private void Update()
    {
        CheckWaypointDestination();
        SetNextPosition(_currentStepSize);

        if (Input.GetKey(_a) || Input.GetKey(_leftArrow))
        {
            _currentStepSize -= 8f * Time.deltaTime;
        }
        else if(Input.GetKey(_d) || Input.GetKey(_rightArrow))
        {
            _currentStepSize += 8f * Time.deltaTime;
        }

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).position.x < _screenWidght / 2)
            {
                _currentStepSize -= 18f * Time.deltaTime;
            }
            else if (Input.GetTouch(0).position.x > _screenWidght / 2)
            {
                _currentStepSize += 18f * Time.deltaTime;
            }
        }

        _currentStepSize = Mathf.Clamp(_currentStepSize, -_stepSize, _stepSize);
    }
    
    private void SetNextPosition(float stepSize)
    {
        float length = Vector3.Distance(_playerMover.transform.position, waypointStart) + _lengthFromVehicle;
        _transform.position = waypointStart + waypointDirection * length + waypointRightDirection * stepSize;
    }
}