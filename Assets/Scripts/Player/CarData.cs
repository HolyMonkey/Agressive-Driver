using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new Car", menuName = "Car Data", order = 51)]
public class CarData : ScriptableObject
{
    [SerializeField] private Player _carPrefab;
    [SerializeField] private Sprite _carIcon;

    public Player CarPrefab { get { return _carPrefab; } }
    public Sprite CarIcon { get { return _carIcon; } }
}
