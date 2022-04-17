using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WalletShower : MonoBehaviour
{
    [SerializeField] private TMP_Text _money;
    [SerializeField] private SaveSystem _saveSystem;
    
    private void OnEnable()
    {
        _saveSystem.MoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        _saveSystem.MoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(int money)
    {
        _money.text = money.ToString();
    }
}
