using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destroyer : MonoBehaviour
{
    public event UnityAction<GameObject> EnemyDestroyed;
    [SerializeField] private CarsSpawner _carsPawner;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            Destroy(enemy.gameObject);//
           // EnemyDestroyed?.Invoke(enemy.gameObject);
            //_carsPawner.OnEnemyDestroyed(enemy.gameObject);
        }
    }
}
