using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] private CarData[] _cars;
    [SerializeField] private Image _carImage;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private StartGameHider _startGameHider;

    private int _carIndex = 0;
    private Image _currentImage;

    public event UnityAction<Player> CarSelected;

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
        _startGameHider.Show();
        gameObject.SetActive(false);     
    }
}

