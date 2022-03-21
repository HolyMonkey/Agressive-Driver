using UnityEngine;
using UnityEngine.Events;

public class PlayerChanger : MonoBehaviour
{
    [SerializeField] private Player _currentPlayer;
    [SerializeField] private TargetPoint _targetPoint;
    [SerializeField] private Skidmarks _skidMarks;
    [SerializeField] private PlayerSelector _playerSelector;

    public event UnityAction<Player> PlayerChanged;
    public event UnityAction<PlayerMover> PlayerMoverChanged;
    public event UnityAction<PlayerSlower> PlayerSlowerChanged;
    public event UnityAction<Transform> PlayerTransformChanged;
    public event UnityAction<Rigidbody> PlayerRigidbodyChanged;
    public event UnityAction<NearMissChecker> NearMissCheckerChanged;

    private PlayerMover _currentPlayerMover;

    public PlayerMover CurrentPlayerMover => _currentPlayerMover;

    private void OnEnable()
    {
        _playerSelector.CarSelected += Change;
    }

    private void OnDisable()
    {
        _playerSelector.CarSelected -= Change;
    }

    private void Change(Player playerPrefab)
    {
        Player newPlayer = Instantiate(playerPrefab, _currentPlayer.transform.position, _currentPlayer.transform.rotation);

        PlayerMover playerMover = newPlayer.GetComponent<PlayerMover>();
        PlayerSlower playerSlower = newPlayer.GetComponent<PlayerSlower>();
        PlayerVehicleAI playerVehicleAI = newPlayer.GetComponent<PlayerVehicleAI>();
        NearMissChecker nearMissChecker = newPlayer.GetComponentInChildren<NearMissChecker>();
        Rigidbody playerRigidbody = newPlayer.GetComponent<Rigidbody>();
        Transform playerTransform = newPlayer.transform;

        Destroy(_currentPlayer.gameObject);

        _currentPlayer = newPlayer;
        _currentPlayerMover = playerMover;
        playerVehicleAI.SetTargetPositionTransform(_targetPoint.transform);
        playerMover.SetCarSteer(_skidMarks.transform);
        
        PlayerChanged?.Invoke(newPlayer);
        PlayerMoverChanged?.Invoke(playerMover); 
        PlayerSlowerChanged?.Invoke(playerSlower);
        PlayerTransformChanged?.Invoke(playerTransform);
        PlayerRigidbodyChanged?.Invoke(playerRigidbody);
        NearMissCheckerChanged?.Invoke(nearMissChecker);
    }   
}
