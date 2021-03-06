using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : MonoBehaviour
{
    [SerializeField] private ParticleSystem _pickUpEffect;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player player))
        {
            player.Repair();

            Instantiate(_pickUpEffect, transform.position, _pickUpEffect.gameObject.transform.rotation);
            Destroy(gameObject, 0.1f);
        }
    }
}
