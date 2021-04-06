using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyOvertakingArea : MonoBehaviour
{
    public event UnityAction EnemyOvertaking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Vector3 _collisionPoint = transform.InverseTransformPoint(other.transform.position).normalized;
            EnemyOvertaking?.Invoke();
            player.Overtook();
        }
    }
}
