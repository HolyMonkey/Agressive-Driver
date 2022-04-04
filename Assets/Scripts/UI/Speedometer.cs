using UnityEngine;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private float _startZRotation;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private Transform _arrow;
    [SerializeField] private Rigidbody _rigidbodyCar;
    [SerializeField] private float _maxZRotation = -90;

    private float _currentSpeed;
    
    private void Start()
    {
        _arrow.localRotation = Quaternion.Euler(0, 0, _startZRotation);
    }

    private void FixedUpdate()
    {
        _currentSpeed = _rigidbodyCar.velocity.magnitude;
        
        if (_currentSpeed >= _maxSpeed)
            _currentSpeed = _maxSpeed;
        
        float nextZRotation = GetSpeedRotation();
        _arrow.localRotation = Quaternion.Euler(0, 0, nextZRotation);
    }

    private void OnEnable()
    {
        _playerChanger.PlayerRigidbodyChanged += OnRigidbodyCarChanged;
    }

    private void OnDisable()
    {
        _playerChanger.PlayerRigidbodyChanged += OnRigidbodyCarChanged;
    }

    private void OnRigidbodyCarChanged(Rigidbody newRigidbodyCar)
    {
        _rigidbodyCar = newRigidbodyCar;
    }

    private float GetSpeedRotation()
    {
        float angleSize = _startZRotation - _maxZRotation;
        float normalizedSpeed = _currentSpeed / _maxSpeed;
        float speedRotation = _startZRotation - normalizedSpeed * angleSize;
        return speedRotation;
    }
}