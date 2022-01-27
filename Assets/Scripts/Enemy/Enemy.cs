using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private float _impulseForce;
    [SerializeField] private float _diedTime;
    [SerializeField] private ParticleSystem _diedEffect;

    private Rigidbody _rigidbody;
    private Vector3 _forward;
    private ForceMode _impulse;

    public event UnityAction EnemyDied;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _forward = Vector3.forward;
        _impulse = ForceMode.Impulse;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        if (collision.gameObject.TryGetComponent(out Player player))
        {
            Die(contact.point);
            _diedEffect.Play();
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            if (_hitEffect.TryGetComponent(out ParticleSystem particleSystem))
            {
                Vector3 targetPosition = new Vector3(contact.point.x, _hitEffect.transform.position.y, contact.point.z);
                _hitEffect.transform.position = targetPosition;
                particleSystem.Play();
            }

            Die(contact.point);
            _diedEffect.Play();
        }
        

        if (collision.gameObject.TryGetComponent(out Destroyer destroyer))
        {
            Destroy(gameObject);
        }
    }

    private void Die(Vector3 hitDirection)
    { 
        _rigidbody.AddForce(_forward * _impulseForce, _impulse);
        EnemyDied?.Invoke();
    }
}
