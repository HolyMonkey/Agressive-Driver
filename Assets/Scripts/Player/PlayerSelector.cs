using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] private CarData[] _cars;
    [SerializeField] private Image _carImage;
    [SerializeField] private StartGameHider _startGameHider;
    [Header("Buttons")]
    [SerializeField] private Button _select;
    [SerializeField] private Button _nextCar;
    [SerializeField] private Button _previousCar;

    private ButtonsAnimator _buttonsAnimator;
    private int _carIndex = 0;

    public event Action<Player> CarSelected;
    public event Action<Button> ButtonSelected;

    private void OnEnable()
    {
        _buttonsAnimator = GetComponentInParent<ButtonsAnimator>();

        _select.onClick.AddListener(SelectCar);
        _nextCar.onClick.AddListener(NextCar);
        _previousCar.onClick.AddListener(PreviousCar);
    }

    private void Start()
    {
        _carImage.sprite = _cars[_carIndex].CarIcon;
    }

    private void OnDisable()
    {
        _select.onClick.RemoveListener(SelectCar);
        _nextCar.onClick.RemoveListener(NextCar);
        _previousCar.onClick.RemoveListener(PreviousCar);
    }

    private void NextCar()
    {
        if(_carIndex < _cars.Length - 1)
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
}
