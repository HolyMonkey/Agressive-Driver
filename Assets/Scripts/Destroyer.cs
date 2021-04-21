using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            Destroy(enemy.gameObject,2);//
           // EnemyDestroyed?.Invoke(enemy.gameObject);
            //_carsPawner.OnEnemyDestroyed(enemy.gameObject);
        }
    }
}
