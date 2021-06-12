using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupTextSpawner : MonoBehaviour
{
    [SerializeField] private UITextPool _pool;

    public void SpawnTextPoint(string type, int points)
    {
        if (_pool.TryGetActiveObject(out UITextTemplate text))
        {
            text.Init(type, points);
            text.gameObject.SetActive(true);
        }
    }
}
