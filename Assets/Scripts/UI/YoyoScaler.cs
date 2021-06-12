using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class YoyoScaler : MonoBehaviour
{
    [SerializeField] private float _sizeTo;
    [SerializeField] private float _duration;
    
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _rectTransform.DOScale(_sizeTo, _duration).SetLoops(-1, LoopType.Yoyo);
    }
}