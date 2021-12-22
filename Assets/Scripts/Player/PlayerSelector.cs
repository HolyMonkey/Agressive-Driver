using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] private CarData[] _cars;
    [SerializeField] private Image _carImage;
    [SerializeField] private StartGameHider _startGameHider;

    private ButtonsAnimator _buttonsAnimator;
    private int _carIndex = 0;

    public event UnityAction<Player> CarSelected;

    private void OnEnable()
    {
        _buttonsAnimator = GetComponentInParent<ButtonsAnimator>();
    }

    private void Start()
    {
        _carImage.sprite = _cars[_carIndex].CarIcon;
    }

    public void NextCar()
    {
        if(_carIndex < _cars.Length - 1)
        {
            _carIndex++;
            _carImage.sprite = _cars[_carIndex].CarIcon;
        }   
    }

    public void PreviousCar()
    {
        if (_carIndex > 0)
        {
            _carIndex--;
            _carImage.sprite = _cars[_carIndex].CarIcon;
        }    
    }

    public void SelectCar()
    {
        CarSelected?.Invoke(_cars[_carIndex].CarPrefab);
        _buttonsAnimator.SetFadeDown();
        _startGameHider.Show();
    }
}

