using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] private ParticleSystem _pickUpEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Instantiate(_pickUpEffect, transform.position, _pickUpEffect.gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
