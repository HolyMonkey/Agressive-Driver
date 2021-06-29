using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour 
{
    [SerializeField] private int _health;
    [SerializeField] private AdSettings _ad;

    private readonly int _damage = 25;
    private int _currentHealth;
    private int _startHealth;

    public int Health => _currentHealth;

    public event UnityAction<Vector3> PlayerHited;
    public event UnityAction PlayerDied;
    public event UnityAction PlayerOvertook;
    public event UnityAction<int, int> HealthChanged;
    public event UnityAction<Enemy> Boarding;
    public event Action Revived;

    private void OnEnable()
    {
        _ad.AdShowned += Revive;
    }

    private void OnDisable()
    {
        _ad.AdShowned -= Revive;
    }

    private void Start()
    {
        _currentHealth = _health;
        _startHealth = _health;
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

    public void Revive()
    {
        _currentHealth = _startHealth;
        Revived?.Invoke();
    }
    
    private void Die()
    {
        if (enabled)
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

            if (_currentHealth > 0 && Mathf.Abs(collisionDirection.x) >= 0.85f)
                Boarding?.Invoke(enemy);
        }
    }
}