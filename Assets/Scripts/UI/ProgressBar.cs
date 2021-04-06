using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private Player _player;
    [SerializeField] private Destroyer _destroyer;
    [SerializeField] private FinishSpawner _finishSpawner;
    [SerializeField] private Slider _progressSlider;

    private void OnEnable()
    {
        _finishSpawner.PlayerReached += OnPlayerReaachedFinish;
    }

    private void OnDisable()
    {
        _finishSpawner.PlayerReached -= OnPlayerReaachedFinish;
    }

    private void Update()
    {
        _progressSlider.value = _finishSpawner.Complete;
    }

    private void OnPlayerReaachedFinish()
    {
       // _camera.ShowFinish();
        _destroyer.GetComponent<BoxCollider>().enabled = false;
    }
}
