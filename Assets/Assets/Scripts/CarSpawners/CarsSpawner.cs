using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsSpawner : ObjectPool
{
    [SerializeField] private GameObject[] _carsPrefabs;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Destroyer _destroyer;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _minDstanceBetweenSpawn;
    [SerializeField] private float _maxDstanceBetweenSpawn;
    [SerializeField] private Player _player;

    private readonly float _startSpawnDstance = 100f;
    private float _spawnPositionZ;

    private void Start()
    {
        _spawnPositionZ = _startSpawnDstance;
    }

    private void Update()
    {
        if (_player.transform.position.z >= _spawnPositionZ)
        {
                _spawnPositionZ = _player.transform.position.z + Random.Range(_minDstanceBetweenSpawn, _maxDstanceBetweenSpawn);

                int indexOfCar = Random.Range(0, _carsPrefabs.Length);
                int indexOfPoint = Random.Range(0, _spawnPoints.Length);

                SpawnCar(_carsPrefabs[indexOfCar], _spawnPoints[indexOfPoint].position);
        }
    }

    private void SpawnCar(GameObject car, Vector3 spawnPoint)
    {
        Instantiate(car, new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z + _spawnPositionZ + _startSpawnDstance), car.transform.rotation);
    }
}
