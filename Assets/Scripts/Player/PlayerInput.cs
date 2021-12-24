using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private TargetPoint _targetPoint;
    [SerializeField] private float _doubleClickTime;
    
    private PlayerMover _playerMover;
    private float _lastClickTime;

    private void Start()
    {
        _playerMover = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            float _timeSinceLastClick = Time.time - _lastClickTime;

            _lastClickTime = Time.time;
        }
    }
}
