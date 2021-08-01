using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStartGame : MonoBehaviour
{
    [SerializeField] private List<GameObject> _needShowObject;
    [SerializeField] private List<CarSpawner> _spawners;
    [SerializeField] private Rigidbody _rigidbodyPlayer;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private StartGameHider _startGameHider;
    [SerializeField] private PlayerSelector _playerSelector;

    private bool _isCarSelected;

    private void Start()
    {
        _startGameHider.Hide();
    }

    private void Update()
    {
        if (_isCarSelected == true)
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
                _startGameHider.Hide();
            }
        }
    }

    private void OnEnable()
    {
        _playerChanger.PlayerRigidbodyChanged += OnPlayerRigidbodyChanged;
        _playerSelector.CarSelected += OnCarSelected;
    }

    private void OnDisable()
    {
        _playerChanger.PlayerRigidbodyChanged -= OnPlayerRigidbodyChanged;
        _playerSelector.CarSelected -= OnCarSelected;
    }

    private void OnCarSelected(Player selectedPlayer)
    {
        _isCarSelected = true;
    }

    private void OnPlayerRigidbodyChanged(Rigidbody newRigidbodyPlayer)
    {
        _rigidbodyPlayer = newRigidbodyPlayer;
    }
}
