using System;
using UnityEngine;
using UnityEngine.UI;
using YandexGames;
using TMPro;
using System.Collections;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] private CarData[] _cars;
    [SerializeField] private Image _carImage;
    [SerializeField] private StartGameHider _startGameHider;
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
        _movementInstruction.onClick.AddListener(GetMovementInstraction);
        _closeInstruction.onClick.AddListener(CloseInstruction);
    }

    private IEnumerator Start()
    {
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
        _movementInstruction.onClick.RemoveListener(GetMovementInstraction);
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
        CarSelected?.Invoke(_cars[_carIndex].CarPrefab);
        _buttonsAnimator.SetFadeDown();
        _startGameHider.Show();
        ButtonSelected?.Invoke(_select);
    }

    private void GetLeaderBoard()
    {
        _leaderboardPanel.SetActive(true);

        Leaderboard.GetEntries("PlaytestBoard", (result) =>
        {
            var entries = result.entries;

            foreach (var entry in entries)
            {
                string name = entry.player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = "Anonymous";

                int score = entry.score;
                string playerScore = $"{entry.rank} {name} {score}\n";

                _panelText.text += playerScore;
            }
        });
    }

    private void GetMovementInstraction()
    {
        _instructionPanel.SetActive(true);
        _instructionText.gameObject.SetActive(true);
    }

    private void CloseLeaderboard()
    {
        _panelText.text = string.Empty;
        _leaderboardPanel.SetActive(false);
    }

    private void CloseInstruction()
    {
        _instructionText.gameObject.SetActive(false);
        _instructionPanel.SetActive(false);
    }
}
