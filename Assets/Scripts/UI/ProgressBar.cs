using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform _finish;
    [SerializeField] private Slider _progressSlider;
    private float distance;
    private float lastProgress = 0;

    private void Start()
    {
        distance = Vector3.Distance(_player.transform.position, _finish.transform.position);
    }

    private void Update()
    {
        var progress = (1 - Vector3.Distance(_player.transform.position, _finish.transform.position) / distance);
        if (progress > lastProgress)
            lastProgress = progress;
        _progressSlider.value = lastProgress;
    }
}