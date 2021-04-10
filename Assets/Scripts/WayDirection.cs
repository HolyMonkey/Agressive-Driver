using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayDirection : MonoBehaviour
{
    [SerializeField] private TargetPoint _targetPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMover>())
            _targetPoint.SetWayDirection(transform);
    }
}
