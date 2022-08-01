using System;
using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DeviceType = Agava.YandexGames.DeviceType;

public class UIStartGame : MonoBehaviour
{
    [SerializeField] private List<CarSpawner> _spawners;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private PlayerSelector _playerSelector;
    [SerializeField] private ButtonsAnimator _buttonsAnimator;
    [SerializeField] private GameObject _phoneButtons;
    [SerializeField] private GameObject _speedometerForPC;
    [SerializeField] private GameObject _speedometerForPhone;

    private CanvasGroup _canvasGroup;
    private Rigidbody _rigidbodyPlayer;
    private bool _isCarSelected;
    private bool _isGameStarted;

    public event UnityAction GameStarted;
    
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (_isGameStarted == false && _isCarSelected)
        {
            if (Input.GetMouseButtonDown(0) && _canvasGroup.interactable)
            {
                _rigidbodyPlayer.isKinematic = false;
                _isGameStarted = true;
                GameStarted?.Invoke();
                _playerChanger.SetActiveControl();
                
                foreach (var spawner in _spawners)
                {
                    spawner.ActivateCars();
                }
                
                if (Device.Type == DeviceType.Mobile)
                {
                    _phoneButtons.SetActive(true);
                    _speedometerForPhone.SetActive(true);
                }           
                else             
                {
                    _speedometerForPC.SetActive(true);
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
