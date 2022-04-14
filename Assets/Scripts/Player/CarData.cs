using UnityEngine;

[CreateAssetMenu(fileName = "new Car", menuName = "Car Data", order = 51)]
public class CarData : ScriptableObject
{
    [SerializeField] private Player _carPrefab;
    [SerializeField] private Sprite _carIcon;
    [SerializeField] private int _price;

    public Player CarPrefab => _carPrefab;
    public Sprite CarIcon => _carIcon;
    public int Price => _price;
}
