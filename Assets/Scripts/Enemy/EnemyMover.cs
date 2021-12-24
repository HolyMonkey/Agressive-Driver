using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    public int WaypointIndex => _waypointIndex;

    private Vector3 _targetPoint;

    public List<Vector3> waypoints;
    private Vector3 _waypointStart;
    private Vector3 _waypointEnd;
    private Vector3 _waypointDirection;
    private int _waypointIndex;
    private float _lastDistance;


    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private EnemyOvertakingArea _enemyOvertakingArea;
    [SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
    [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
    [SerializeField] private float m_MaximumSteerAngle;
    [SerializeField] private Vector3 m_CentreOfMassOffset;
    [SerializeField] private float _lengthFromVehicle;

    private Quaternion[] m_WheelMeshLocalRotations;
    private float _steerAngle;


    private readonly float _diedSpeed = 0f;

    private Rigidbody _rigidbody;
    private Enemy _enemy;
    private bool _isEnemyDied;
    private Transform _transform;
    private float _fixedDeltaTime;

    private void Awake()
    {
        _fixedDeltaTime = 0.02f;
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _enemy = GetComponent<Enemy>();

        _isEnemyDied = false;

        m_WheelMeshLocalRotations = new Quaternion[4];

        for (int i = 0; i < 4; i++)
        {
            m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
        }

        m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;
    }

    public void SetWaypoint(int index)
    {
        if (_waypointIndex + 1 >= waypoints.Count)
        {
            enabled = false;
            Destroy(gameObject);
            return;
        }

        _waypointIndex = index;
        _waypointStart = waypoints[_waypointIndex];
        _waypointIndex++;
        _waypointEnd = waypoints[_waypointIndex];

        _waypointDirection = (_waypointEnd - _waypointStart).normalized;

        _lastDistance = Vector3.Distance(_transform.position, _waypointEnd);
    }

    private void CheckWaypointDestination()
    {
        var distance = Vector3.Distance(_transform.position, _waypointEnd);
        if (distance > _lastDistance)
        {
            SetWaypoint(_waypointIndex);
        }
        else
            _lastDistance = distance;
    }

    private void OnEnable()
    {
        _enemy.EnemyDied += OnEnemyDied;
        _enemyOvertakingArea.EnemyOvertaking += OnEnemyOvertaking;
    }

    private void OnDisable()
    {
        _enemy.EnemyDied -= OnEnemyDied;
        _enemyOvertakingArea.EnemyOvertaking -= OnEnemyOvertaking;
    }

    private void FixedUpdate()
    {
        CheckWaypointDestination();

        float length = Vector3.Distance(_transform.position, _waypointStart) + _lengthFromVehicle;
        _targetPoint = _waypointStart + _waypointDirection * length;

        _targetPoint.y = _transform.position.y;
        var dir = (_targetPoint - _transform.position).normalized;


        _transform.forward = Vector3.Lerp(_transform.forward, dir, _fixedDeltaTime * 20.4f);
        Vector3 velocity = _transform.forward * _speed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;

        if (_isEnemyDied)
        {
            if (_speed > _diedSpeed)
                _speed -= Time.deltaTime;

            if (_speed <= _diedSpeed)
                _speed = _diedSpeed;
        }
    }

    private void OnEnemyOvertaking()
    {
        _isEnemyDied = true;
    }

    public void HitAvoiding()
    {
        _steerAngle = 1 * m_MaximumSteerAngle;
        m_WheelColliders[0].steerAngle = _steerAngle;
        m_WheelColliders[1].steerAngle = _steerAngle;
    }

    private void OnEnemyDied()
    {
        _isEnemyDied = true;
        _enemyOvertakingArea.gameObject.SetActive(false);
    }
}