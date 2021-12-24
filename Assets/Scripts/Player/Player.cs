using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health;

    private readonly int _damage = 25;
    private int _currentHealth;
    private int _startHealth;
    private Vector3 _beforeDiePosition;
    private Quaternion _beforeDieRotation;

    public int Health => _currentHealth;

    public event UnityAction<Vector3> PlayerHited;
    public event UnityAction PlayerDied;
    public event UnityAction PlayerOvertook;
    public event UnityAction<int, int> HealthChanged;
    public event UnityAction<Enemy> Boarding;
    public event Action DelayRevived;
    public event Action Revived;
    public event Action<int> HealthChanging;

    private void Start()
    {
        _currentHealth = _health;
        _startHealth = _health;
        HealthChanged?.Invoke(_currentHealth, _health);
    }
  
    public void Overtook()
    {
        PlayerOvertook?.Invoke();
    }

    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        HealthChanged?.Invoke(_currentHealth, _health);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator ReviveDelay()
    {
        yield return new WaitForSeconds(1f);
        _currentHealth = _startHealth;
        HealthChanged?.Invoke(_currentHealth, _health);
        DelayRevived?.Invoke();
    }

    private void Die()
    {
        if (enabled)
        {
            _beforeDiePosition = transform.position;
            _beforeDieRotation = transform.rotation;
            PlayerDied?.Invoke();
            _currentHealth = 0;
            HealthChanged?.Invoke(_currentHealth, _health);
        }
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 collisionDirection = collision.GetContact(0).normal;
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            if (Mathf.Abs(collisionDirection.z) >= 0.95f)
            {
                Die();
            }

            PlayerHited?.Invoke(collisionDirection);
            TakeDamage(_damage);
            if (_currentHealth > 0 && Mathf.Abs(collisionDirection.x) >= 0.85f)
            {
                Boarding?.Invoke(enemy);
            }
        }
    }

    public void GetSecondChance()
    {
        _currentHealth = _health;
        _startHealth = _health;
        HealthChanged?.Invoke(_currentHealth, _health);
        transform.position = _beforeDiePosition;
        transform.rotation = _beforeDieRotation;
    }
}