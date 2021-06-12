using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextTemplate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _typeText;
    [SerializeField] private TextMeshProUGUI _pointsText;

    public TextMeshProUGUI TypeText => _typeText;
    public TextMeshProUGUI PointsText => _pointsText;
    
    public void Init(string type, int points)
    {
        _typeText.text = type;
        _pointsText.text = "+" + points;
    }
}
