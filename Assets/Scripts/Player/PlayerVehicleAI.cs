using UnityEngine;

public class PlayerVehicleAI : MonoBehaviour
{
    [SerializeField] private Transform _targetPositionTransform;

    private PlayerMover _vehicleControl;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _vehicleControl = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        SetTargetPosition(_targetPositionTransform.position);
        _vehicleControl.SetInputs(_targetPosition);
    }

    public void SetTargetPositionTransform(Transform newTargetPositionTransform)
    {
        _targetPositionTransform = newTargetPositionTransform;
    }

    private void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}