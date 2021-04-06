using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishSpawner : MonoBehaviour
{
    [SerializeField] private float _distance = 2000;
    [SerializeField] private float _spawnOffset = 100;
    [SerializeField] private Finish _finishPrefab;

    private bool _spawned = false;

    public float Complete => transform.position.z / _distance;
    public event UnityAction PlayerReached;

    private void FixedUpdate()
    {
        if (!_spawned && transform.position.z + _spawnOffset >= _distance)
        {
            Finish finish = Instantiate(_finishPrefab, transform.position + Vector3.forward * _spawnOffset, Quaternion.identity);
            finish.Entered += PlayerReachedFinish;
            _spawned = true;
        }
    }

    private void PlayerReachedFinish()
    {
        PlayerReached?.Invoke();
    }
}
