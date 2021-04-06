using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMover))]
public class EnemyHitAvoiding : MonoBehaviour
{
    [SerializeField] private float _checkDistance;
    [SerializeField] private LayerMask _layerMask;

    private EnemyMover _enemyMover;

    public UnityAction HitAvoiding;

    private void Awake()
    {
        _enemyMover = GetComponent<EnemyMover>();
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z),
                transform.TransformDirection(Vector3.forward), out hit, _layerMask))
        {
            if (hit.distance <= _checkDistance)
            {
                _enemyMover.HitAvoiding();
            }
        }
    }
}
