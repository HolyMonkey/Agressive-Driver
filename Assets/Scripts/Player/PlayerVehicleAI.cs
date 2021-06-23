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
        SetTargetPosition(_targetPositionTranform.position);
        _vehicleControl.SetInputs(_targetPosition);
    }

    private void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}