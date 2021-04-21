using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStartGame : MonoBehaviour
{
    [SerializeField] List<GameObject> _needShowObject;
    [SerializeField] List<CarSpawner> _spawners;
    [SerializeField] Rigidbody _rigidbodyPlayer;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            _rigidbodyPlayer.isKinematic = false;
            foreach (var item in _needShowObject)
            {
                item.SetActive(true);                
            }

            foreach (var spawner in _spawners)
            { 
                spawner.ActivateCars();
            }
            gameObject.SetActive(false);

        }
    }
}
