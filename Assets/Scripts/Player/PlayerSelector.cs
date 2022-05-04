using System;
using UnityEngine;
using UnityEngine.UI;
using Agava.YandexGames;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] private List<CarData> _cars;
    [SerializeField] private Image _carImage;
    [Header("Buttons")] 
    [SerializeField] private Button _buyCarButton;
    [SerializeField] private Button _select;
    [SerializeField] private Button _nextCar;
    [SerializeField] private Button _previousCar;
    [SerializeField] private Button _leaderboard;
    [SerializeField] private Button _movementInstruction;
    [SerializeField] private Button _closeInstruction;
    [SerializeField] private Button _closePanel;
    [SerializeField] private GameObject _leaderboardPanel;
    [SerializeField] private GameObject _instructionPanel;
    [SerializeField] private TMP_Text _panelText;
    [SerializeField] private TMP_Text _carPriceText;
    [SerializeField] private TMP_Text _instructionText;
    [SerializeField] private Leaderboard _leaderboardScript;
    [SerializeField] private SaveSystem _saveSystem;
    [SerializeField] private Color _availableButtonColor;
    [SerializeField] private Color _unavailableButtonColor;
    [SerializeField] private Image _padlockImage;
    [SerializeField] private YoyoScaler _buyButtonTextYoyoScaler;

    private ButtonsAnimator _buttonsAnimator;
    private int _carIndex = 0;

    public event Action<Player> CarSelected;
    public event Action<int,int> CarPurchased;
    public event Action<Button> ButtonSelected;

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private void OnEnable()
    {
        _buttonsAnimator = GetComponentInParent<ButtonsAnimator>();

        _saveSystem.PlayerDataLoaded += OnPlayerDataLoaded;
        _select.onClick.AddListener(SelectCar);
        _nextCar.onClick.AddListener(NextCar);
        _previousCar.onClick.AddListener(PreviousCar);
        _leaderboard.onClick.AddListener(GetLeaderBoard);
        _closePanel.onClick.AddListener(CloseLeaderboard);
        _movementInstruction.onClick.AddListener(GetMovementInstructions);
        _closeInstruction.onClick.AddListener(CloseInstruction);
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif

        // Always wait for it if invoking something immediately in the first scene.
        yield return YandexGamesSdk.WaitForInitialization();
        
        if (PlayerPrefs.HasKey("CarIndex"))
        {
            _carIndex = PlayerPrefs.GetInt("CarIndex");
        }
        
        _instructionText.gameObject.SetActive(false);
        _leaderboardPanel.SetActive(false);
    }

    private void OnDisable()
    {
        _saveSystem.PlayerDataLoaded -= OnPlayerDataLoaded;
        _select.onClick.RemoveListener(SelectCar);
        _nextCar.onClick.RemoveListener(NextCar);
        _previousCar.onClick.RemoveListener(PreviousCar);
        _leaderboard.onClick.RemoveListener(GetLeaderBoard);
        _closePanel.onClick.RemoveListener(CloseLeaderboard);
        _movementInstruction.onClick.RemoveListener(GetMovementInstructions);
        _closeInstruction.onClick.RemoveListener(CloseInstruction);
    }

    public void TryToBuyCar()
    {
        if (_saveSystem.Money >= _cars[_carIndex].Price)
        {
            CarPurchased?.Invoke(_carIndex,_cars[_carIndex].Price);
            ResetCarInfo();
        }
    }

    private void NextCar()
    {
        if (_carIndex < _cars.Count - 1)
        {
            _carIndex++;
            ResetCarInfo();
        }

        ButtonSelected?.Invoke(_nextCar);
    }

    private void PreviousCar()
    {
        if (_carIndex > 0)
        {
            _carIndex--;
            ResetCarInfo();
        }

        ButtonSelected?.Invoke(_previousCar);
    }

    private void SelectCar()
    { 
        if (CheckForCarPurchased())     
        {
            PlayerPrefs.SetInt("CarIndex", _carIndex);
            PlayerPrefs.Save();
            CarSelected?.Invoke(_cars[_carIndex].CarPrefab);
            _buttonsAnimator.DisablePlayerSelector();
            ButtonSelected?.Invoke(_select);
        }
    }

    private void ResetCarInfo()
    {
        _carImage.sprite = _cars[_carIndex].CarIcon;
        
        if (_saveSystem.UnlockedCars.Contains(_carIndex))
        {
            _buyCarButton.gameObject.SetActive(false);
            _carPriceText.gameObject.SetActive(false);
            _padlockImage.gameObject.SetActive(false);
        }
        else
        {
            _carPriceText.gameObject.SetActive(true);
            _carPriceText.text = _cars[_carIndex].Price.ToString();
            _buyCarButton.gameObject.SetActive(true);
            _padlockImage.gameObject.SetActive(true);

            if (_saveSystem.Money >= _cars[_carIndex].Price)
            {
                _buyCarButton.image.color = _availableButtonColor;
                _buyCarButton.interactable = true;
                _buyButtonTextYoyoScaler.enabled = true;
            }
            else
            {
                _buyCarButton.image.color = _unavailableButtonColor;
                _buyCarButton.interactable = false;
                _buyButtonTextYoyoScaler.enabled = false;
            }
        }
    }

    private void GetLeaderBoard()
    {
        _leaderboardScript.Show();
    }

    private void GetMovementInstructions()
    {
        _instructionPanel.SetActive(true);
        _instructionText.gameObject.SetActive(true);
    }

    private void CloseLeaderboard()
    {
        _leaderboardScript.Close();
    }

    private void CloseInstruction()
    {
        _instructionText.gameObject.SetActive(false);
        _instructionPanel.SetActive(false);
    }
    
    private bool CheckForCarPurchased()
    {
        bool purchased = false;
        
        foreach (var car in _saveSystem.UnlockedCars)
            if (car == _carIndex)
            {
                purchased = true;
            }

        return purchased;
    }

    private void OnPlayerDataLoaded()
    {
        ResetCarInfo();
        
        if (_saveSystem.UnlockedCars.Contains(0) == false)
        {
            CarPurchased?.Invoke(_carIndex,_cars[_carIndex].Price);
        }
    }
}
