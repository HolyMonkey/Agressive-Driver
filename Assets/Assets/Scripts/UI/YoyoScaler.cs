using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class YoyoScaler : MonoBehaviour
{
    [SerializeField] private float _sizeTo;
    [SerializeField] private float _duration;
    
    private RectTransform _rectTransform;
    private Vector3 _startScale;
    private Tweener _tweener;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startScale = _rectTransform.localScale;
    }

    private void OnEnable()
    {
        _tweener = _rectTransform.DOScale(_sizeTo, _duration).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        _tweener.Kill();
        _rectTransform.localScale = _startScale;
    }
}