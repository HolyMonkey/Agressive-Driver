using System.Collections.Generic;
using UnityEngine;

public class UIDisabler : MonoBehaviour
{
    [SerializeField] private FinishLevel _finishLevel;
    [SerializeField] private List<GameObject> _panelToDisable;

    private void OnEnable()
    {
        _finishLevel.Finished += OnLevelFinished;
    }

    private void OnDisable()
    {
        _finishLevel.Finished -= OnLevelFinished;
    }

    private void OnLevelFinished()
    {
        foreach (var panel in _panelToDisable)
        {
            panel.SetActive(false);
        }
    }
}