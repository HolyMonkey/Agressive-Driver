using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMover))]
public class EnemyHitAvoiding : MonoBehaviour
{
    [SerializeField] private float _checkDistance;
    [SerializeField] private LayerMask _layerMask;

    private Transform _transform;
    private EnemyMover _enemyMover;
    private Vector3 _forward;

    public UnityAction HitAvoiding;

    private void Awake()
    {
        _enemyMover = GetComponent<EnemyMover>();
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(new Vector3(_transform.position.x, _transform.position.y + 0.5f, _transform.position.z),
                _transform.TransformDirection(_forward), out hit, _layerMask))
        {
            if (hit.distance <= _checkDistance)
            {
                _enemyMover.HitAvoiding();
            }
        }
    }
}
