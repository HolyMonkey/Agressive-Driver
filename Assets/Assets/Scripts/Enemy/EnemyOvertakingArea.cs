using UnityEngine;
using UnityEngine.Events;

public class EnemyOvertakingArea : MonoBehaviour
{
    public event UnityAction EnemyOvertaking;

    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Vector3 _collisionPoint = _transform.InverseTransformPoint(other.transform.position).normalized;
            EnemyOvertaking?.Invoke();
            player.Overtook();
        }
    }
}
