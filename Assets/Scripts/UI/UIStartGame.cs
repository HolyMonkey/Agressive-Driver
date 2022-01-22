using System.Collections.Generic;
using UnityEngine;

public class UIStartGame : MonoBehaviour
{
    [SerializeField] private List<CarSpawner> _spawners;
    [SerializeField] private Rigidbody _rigidbodyPlayer;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private PlayerSelector _playerSelector;
    [SerializeField] private ButtonsAnimator _buttonsAnimator;

    private bool _isCarSelected;

    private void Update()
    {
        if (_isCarSelected == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _rigidbodyPlayer.isKinematic = false;
                for (int i = 0; i < _spawners.Count; i++)
                {
                    _spawners[i].ActivateCars();
                }

                _buttonsAnimator.DisableStart();
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
