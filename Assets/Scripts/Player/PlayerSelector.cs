using System;
using UnityEngine;
using UnityEngine.UI;
using Agava.YandexGames;
using TMPro;
using System.Collections;
using UnityEngine.Serialization;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] private CarData[] _cars;
    [SerializeField] private Image _carImage;
    [Header("Buttons")]
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
    [SerializeField] private TMP_Text _instructionText;
    [SerializeField] private LeaderBoard _leaderboardScript;

    private ButtonsAnimator _buttonsAnimator;
    private int _carIndex = 0;

    public event Action<Player> CarSelected;
    public event Action<Button> ButtonSelected;

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private void OnEnable()
    {
        _buttonsAnimator = GetComponentInParent<ButtonsAnimator>();

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
        if (PlayerPrefs.HasKey("CarIndex"))
        {
            _carIndex = PlayerPrefs.GetInt("CarIndex");
        }
        
        _instructionText.gameObject.SetActive(false);
        _carImage.sprite = _cars[_carIndex].CarIcon;
        _leaderboardPanel.SetActive(false);

        if (PlayerPrefs.HasKey("AllScore"))
        {
            int score = PlayerPrefs.GetInt("AllScore");

            Leaderboard.SetScore("PlaytestBoard", score);
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif

        // Always wait for it if invoking something immediately in the first scene.
        yield return YandexGamesSdk.WaitForInitialization();
    }

    private void OnDisable()
    {
        _select.onClick.RemoveListener(SelectCar);
        _nextCar.onClick.RemoveListener(NextCar);
        _previousCar.onClick.RemoveListener(PreviousCar);
        _leaderboard.onClick.RemoveListener(GetLeaderBoard);
        _closePanel.onClick.RemoveListener(CloseLeaderboard);
        _movementInstruction.onClick.RemoveListener(GetMovementInstructions);
        _closeInstruction.onClick.RemoveListener(CloseInstruction);
    }

    private void NextCar()
    {
        if (_carIndex < _cars.Length - 1)
        {
            _carIndex++;
            _carImage.sprite = _cars[_carIndex].CarIcon;
        }

        ButtonSelected?.Invoke(_nextCar);
    }

    private void PreviousCar()
    {
        if (_carIndex > 0)
        {
            _carIndex--;
            _carImage.sprite = _cars[_carIndex].CarIcon;
        }

        ButtonSelected?.Invoke(_previousCar);
    }

    private void SelectCar()
    {
        PlayerPrefs.SetInt("CarIndex",_carIndex);
        PlayerPrefs.Save();
        CarSelected?.Invoke(_cars[_carIndex].CarPrefab);
        _buttonsAnimator.DisablePlayerSelector();
        ButtonSelected?.Invoke(_select);
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
}
