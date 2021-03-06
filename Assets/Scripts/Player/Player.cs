using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour 
{
    [SerializeField] private int _health;

    private int _currentHealth;
    readonly private int _damage = 25;

    public event UnityAction<Vector3> PlayerHited;
    public event UnityAction PlayerDied;
    public event UnityAction PlayerOvertook;
    public event UnityAction<int, int> HealthChanged;

    private void Start()
    {
        _currentHealth = _health;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        HealthChanged?.Invoke(_currentHealth, _health);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerDied?.Invoke();
    }

    public void Overtook()
    {
        PlayerOvertook?.Invoke();
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
        }
    }

    public void Repair()
    {
        _currentHealth = _health;

        HealthChanged?.Invoke(_currentHealth, _health);
    }
}