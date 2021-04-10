using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    public Transform wayDirection;
    private Vector3 targetPoint;

    public List<Vector3> waypoints;
    private Vector3 waypointStart;
    private Vector3 waypointEnd;
    private Vector3 waypointDirection;
    private int waypointIndex;
    private float lastDistance;


    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private EnemyOvertakingArea _enemyOvertakingArea;
    [SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
    [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
    [SerializeField] private float m_MaximumSteerAngle;
    [SerializeField] private Vector3 m_CentreOfMassOffset;
    [SerializeField] private Transform _target;
    [SerializeField] private float _targetLineX;
    [SerializeField] private float _lengthFromVehicle;
    [SerializeField] private float _collisionOffsetX;

    private Quaternion[] m_WheelMeshLocalRotations;
    private float _steerAngle;


    private readonly float _diedSpeed = 0f;

    private Rigidbody _rigidbody;
    private Enemy _enemy;
    private bool _isEnemyDied;

    private void Awake()
    {
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
        waypointIndex = index;
        waypointStart = waypoints[waypointIndex];
        waypointIndex++;
        if (waypointIndex == waypoints.Count)
        {
            enabled = false;
            return;
        }
        waypointEnd = waypoints[waypointIndex];

        waypointDirection = (waypointEnd - waypointStart).normalized;

        lastDistance = Vector3.Distance(transform.position, waypointEnd);
    }

    private void CheckWaypointDestination()
    {
        var distance = Vector3.Distance(transform.position, waypointEnd);
        if (distance > lastDistance)
        {
            SetWaypoint(waypointIndex);
        }
        else
            lastDistance = distance;
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

        float length = Vector3.Distance(transform.position, waypointStart) + _lengthFromVehicle;
        targetPoint = waypointStart + waypointDirection * length;

        targetPoint.y = 0;
        var dir = (targetPoint - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward , dir, Time.fixedDeltaTime * 2.4f);
        Vector3 velocity = transform.forward * _speed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;

        if (_isEnemyDied)
        {
            if(_speed > _diedSpeed)
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
            _targetLineX = -10F;

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