using EasyRoads3Dv3;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private List<GameObject> _carsTemplate;    
    [SerializeField] bool isBackDirection;
    [SerializeField] private float minDistance = 80;
    [SerializeField] private float maxDistance = 100;
    private float distanceForNextSpawn = 100;

    [SerializeField] private ERModularRoad eRoad;

    private List<Vector3> waypoints = new List<Vector3>();
    private List<Vector3> waypointsForCar = new List<Vector3>();
    private List<EnemyMover> _cars = new List<EnemyMover>();
    private bool _isStartLevel = false;
    private EnemyMover _lastCar;
    private Vector3 _up;
    private Quaternion _identity;
    private Vector3 _direction;

    private void Start()
    {
        _identity = Quaternion.identity;
        _up = Vector3.up;
        int count = eRoad.soSplinePoints.Count;
        
        for (int i = 0; i < count; i++)
        {
            waypoints.Add(eRoad.soSplinePoints[i] + _up);
        }

        if (isBackDirection)
        {
            OffSetingWayPoints(-1);

            for (int i = 0; i < waypoints.Count; i++)
            {
                waypointsForCar.Add(waypoints[i]);
            }

            waypointsForCar.Reverse();
        }
        else
        {
            OffSetingWayPoints();

            for (int i = 0; i < waypoints.Count; i++)
            {
                waypointsForCar.Add(waypoints[i]);
            }
        }
        SpawnCar(0);
    }

    private void OnEnable()
    {
        _playerChanger.PlayerTransformChanged += OnPlayerTransformChanged;
    }

    private void OnDisable()
    {
        _playerChanger.PlayerTransformChanged -= OnPlayerTransformChanged;
    }

    public void ActivateCars()
    {
        for (int i = 0; i < _cars.Count; i++)
        {
            if (_cars[i] == null)
            {
                _cars.Remove(_cars[i]);
                i--;
            }
            else
            {
                var car = _cars[i];
                car.enabled = true;
            }
        }
        _isStartLevel = true;
    }

    private void SpawnCar(int waypointIndex)
    {
        for (int i = waypointIndex; i < waypoints.Count - 1; i++)
        {

            if (!isBackDirection)
                _direction = (waypoints[i + 1] - waypoints[i]).normalized;
            else
                _direction = (waypoints[i] - waypoints[i + 1]).normalized;

            float distance = Vector3.Distance(waypoints[i], waypoints[i + 1]);

            if (distance < distanceForNextSpawn)
            {
                distanceForNextSpawn -= distance;
            }
            else
            {
                var pos = waypoints[i] + _direction * distanceForNextSpawn + _up * 0.1f;
                var car = Instantiate(_carsTemplate[Random.Range(0, _carsTemplate.Count)], pos, _identity);

                _direction.y = 0;
                car.transform.forward = _direction;
                EnemyMover mover = car.GetComponent<EnemyMover>();
                mover.waypoints = waypointsForCar;

                if (isBackDirection)
                    mover.SetWaypoint(waypoints.Count - i);
                else
                    mover.SetWaypoint(i);

                var diffDistance = distanceForNextSpawn - distance;
                SetNextSpawnDistance();

                mover.enabled = _isStartLevel;
                _cars.Add(mover);
                _lastCar = mover;
                break;
            }
        }
    }
    
    private void FixedUpdate() 
    {
        if (_lastCar != null && Vector3.Distance(_player.position, _lastCar.transform.position) < 200)
        {
            var index = _lastCar.WaypointIndex;
            if (isBackDirection)
                index = waypoints.Count - index;
            SpawnCar(index);
        }
    }

    private void OnPlayerTransformChanged(Transform newPlayerTransform)
    {
        _player = newPlayerTransform;
    }

    private void OffSetingWayPoints()
    {
        List<Vector3> newWaypoints = new List<Vector3>();
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            var dir = (waypoints[i + 1] - waypoints[i]).normalized;
            var waypointRightDirection = Quaternion.AngleAxis(90, _up) * dir;
            newWaypoints.Add( waypoints[i] + waypointRightDirection * 1.4f);
        }
        newWaypoints.Add(waypoints[waypoints.Count - 1]);
        waypoints = newWaypoints;
    }

    private void OffSetingWayPoints(int direction = 1)
    {
        List<Vector3> newWaypoints = new List<Vector3>();
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            var dir = (waypoints[i + 1] - waypoints[i]).normalized;
            var waypointRightDirection = Quaternion.AngleAxis(90, _up) * dir;
            newWaypoints.Add(waypoints[i] + direction * waypointRightDirection * 1.4f);
        }
        newWaypoints.Add(waypoints[waypoints.Count - 1]);
        waypoints = newWaypoints;
    }

    private void SetNextSpawnDistance()
    {
        distanceForNextSpawn = Random.Range(minDistance, maxDistance);
    }
}