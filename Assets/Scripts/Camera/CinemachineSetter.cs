using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
[RequireComponent(typeof(CinemachineVirtualCamera))]

public class CinemachineSetter : MonoBehaviour
{
    [SerializeField] private PlayerChanger _playerChanger;

    private CinemachineVirtualCamera _cinemachine;

    private void OnEnable()
    {
        _playerChanger.PlayerChanged += OnPlayerChanged;
    }

    private void OnDisable()
    {
        _playerChanger.PlayerChanged -= OnPlayerChanged;
    }

    private void Start()
    {
        _cinemachine = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnPlayerChanged(Player player)
    {
        _cinemachine.Follow = player.transform;
        _cinemachine.LookAt = player.transform;
    }
}
