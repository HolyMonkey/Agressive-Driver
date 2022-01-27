using TMPro;
using UnityEngine;
using System.Text;

public class UITextTemplate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _typeText;
    [SerializeField] private TextMeshProUGUI _pointsText;
    private StringBuilder _plus = new StringBuilder("+");

    public TextMeshProUGUI TypeText => _typeText;
    public TextMeshProUGUI PointsText => _pointsText;

    public void Init(string type, int points)
    {
        _typeText.text = type;
        _pointsText.text = _plus + points.ToString();
    }
}
