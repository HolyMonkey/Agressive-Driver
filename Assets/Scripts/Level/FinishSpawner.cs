using UnityEngine;
using UnityEngine.Events;

public class FinishSpawner : MonoBehaviour
{
    [SerializeField] private float _distance = 2000;
    [SerializeField] private float _spawnOffset = 100;
    [SerializeField] private Finish _finishPrefab;

    private bool _spawned = false;
    private Transform _transform;
    private Vector3 _forward;
    private Quaternion _rotation;

    public float Complete => _transform.position.z / _distance;
    public event UnityAction PlayerReached;

    private void Start()
    {
        _rotation = Quaternion.identity;
        _forward = Vector3.forward;
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (!_spawned && _transform.position.z + _spawnOffset >= _distance)
        {
            Finish finish = Instantiate(_finishPrefab, _transform.position + _forward * _spawnOffset, _rotation);
            finish.Entered += PlayerReachedFinish;
            _spawned = true;
        }
    }

    private void PlayerReachedFinish()
    {
        PlayerReached?.Invoke();
    }
}
