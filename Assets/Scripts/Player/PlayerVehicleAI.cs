using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleAI : MonoBehaviour
{

    [SerializeField] private Transform _targetPositionTranform;

    private PlayerMover _vehicleControl;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _vehicleControl = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        Vector3 relativeVector = transform.InverseTransformDirection(_targetPositionTranform.position);
        float newSteer = (relativeVector.x / relativeVector.magnitude);

        SetTargetPosition(_targetPositionTranform.position);

        _vehicleControl.SetInputs(_targetPosition);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
