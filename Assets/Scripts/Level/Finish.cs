using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Finish : MonoBehaviour
{
    public event UnityAction Entered;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<DiscretePlayerMovement>())
    //        Entered?.Invoke();
    //}
}

