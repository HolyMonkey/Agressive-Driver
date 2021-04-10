using EasyRoads3Dv3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{    
    [SerializeField] private List<GameObject> _carsTemplate;
    [SerializeField] bool isBackDirection;
    [SerializeField] private float minDistance = 80;
    [SerializeField] private float maxDistance = 100;
    private float distanceForNextSpawn = 100;

    [SerializeField] private ERModularRoad eRoad;
    private List<Vector3> waypoints = new List<Vector3>();

    private List<Transform> spawnersPoints = new List<Transform>();

    void Start()
    {
        foreach (var vec in eRoad.soSplinePoints)
        {
            waypoints.Add(vec + Vector3.up);
        }

        if (isBackDirection)
            waypoints.Reverse();

        OffsetingWayPoints();

        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            var direction = (waypoints[i + 1] - waypoints[i]).normalized;
            float dist = Vector3.Distance(waypoints[i], waypoints[i + 1]);

            if (dist < distanceForNextSpawn)
            {
                distanceForNextSpawn -= dist;
            }
            else
            {
                var pos = waypoints[i] + direction * distanceForNextSpawn+ Vector3.up * 0.2f;
                var car = Instantiate(_carsTemplate[Random.Range(0, _carsTemplate.Count)], pos, Quaternion.identity);

                direction.y = 0;
                car.transform.forward = direction;
                EnemyMover mover = car.GetComponent<EnemyMover>();
                mover.waypoints = waypoints;
                mover.SetWaypoint(i);

                var diffDistance = distanceForNextSpawn - dist;
                SetNextSpawnDistance();
            }

        }
    }

    public void OffsetingWayPoints()
    {
        List<Vector3> newWaypoints = new List<Vector3>();
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            var dir = (waypoints[i + 1] - waypoints[i]).normalized;
            var waypointRightDirection = Quaternion.AngleAxis(90, Vector3.up) * dir;
            newWaypoints.Add( waypoints[i] + waypointRightDirection * 1.4f);
        }
        newWaypoints.Add(waypoints[waypoints.Count - 1]);
        waypoints = newWaypoints;
    }

    void SetNextSpawnDistance()
    {
        distanceForNextSpawn = Random.Range(minDistance, maxDistance);
    }

    /*
    void Start()
    {
        int index = 0;
        if (isBackDirection) index = 1;
        
        foreach (Transform ts in transform)
        {
            spawnersPoints.Add(ts);
        }

        for (int i = 0; i < spawnersPoints.Count - 1; i++)
        {
            int steps = (int)(Vector3.Distance(spawnersPoints[i].position, spawnersPoints[i + 1].position) / Random.Range(minDistance, maxDistance));

            for (int j = 0; j < steps; j++)
            {
                
                var pos = Vector3.Lerp(spawnersPoints[i].position, spawnersPoints[i+1].position, (float)j / steps + Random.Range(-0.01f, 0.01f));
                var go = Instantiate(_carsTemplate[Random.Range(0, _carsTemplate.Count)], pos, spawnersPoints[i + index].rotation);
                go.GetComponent<EnemyMover>().wayDirection = spawnersPoints[i + index];
            }
        }
    }*/


}