using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Player _target;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private float _smoothSpeed;

    private Vector3 _offset;
    private Vector3 _targetPosition;
    private Transform _transform;
    private float _fixedDeltaTime;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _offset = _transform.position - _target.transform.position;
        _fixedDeltaTime = 0.02f;
    }

    private void OnEnable()
    {
        _playerChanger.PlayerChanged += OnPlayerChange;
    }

    private void OnDisable()
    {
        _playerChanger.PlayerChanged -= OnPlayerChange;
    }

    private void FixedUpdate()
    {
        _targetPosition = _target.transform.position + _offset;

        Vector3 smoothedPosition = Vector3.Lerp(_transform.position, _targetPosition, _smoothSpeed * _fixedDeltaTime);

        _transform.position = smoothedPosition;

        _transform.LookAt(_target.transform);
    }

    private void OnPlayerChange(Player newPlayer)
    {
        _target = newPlayer;
    }
}
