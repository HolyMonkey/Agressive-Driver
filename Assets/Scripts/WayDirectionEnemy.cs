using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayDirectionEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyMover enemyMover))
            enemyMover.wayDirection = transform;
    }


}