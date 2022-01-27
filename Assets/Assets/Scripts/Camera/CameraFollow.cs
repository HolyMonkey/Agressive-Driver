using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Player _target;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private float _smoothSpeed;

    private Vector3 _offset;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private Vector3 _originalPosition;
    private Transform _transform;
    private float _fixedDeltaTime;
    private bool _isReady = false;

    private void Start()
    {
        _transform = GetComponent<Transform>();

        _originalPosition = transform.position;
        _startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 5f);
        _offset = _transform.position - _target.transform.position;
        _fixedDeltaTime = 0.02f;

        transform.position = _startPosition;
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
        if (!_isReady)
        {
            return;
        }
        else
        {
            _targetPosition = _target.transform.position + _offset;

            Vector3 smoothedPosition = Vector3.Lerp(_transform.position, _targetPosition, _smoothSpeed * _fixedDeltaTime);

            _transform.position = smoothedPosition;

            _transform.LookAt(_target.transform);
        }
    }

    private void OnPlayerChange(Player newPlayer)
    {
        _target = newPlayer;
        MoveToStart();
    }

    private void MoveToStart()
    {
        transform.DOMove(_originalPosition, 0.5f).OnComplete(OnStarted);
    }

    private void OnStarted()
    {
        _isReady = true;
    }
}
