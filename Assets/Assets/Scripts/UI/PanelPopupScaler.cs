using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Text;

[RequireComponent(typeof(CanvasGroup))]
public class PanelPopupScaler : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _duration;
    [SerializeField] private Points _points;
    [SerializeField] private TextMeshProUGUI _typeText;
    [SerializeField] private TextMeshProUGUI _pointsText;

    private StringBuilder _plus = new StringBuilder("+");
    private readonly int _endSize = 1;
    private readonly float _startSize = 0.4f;
    private Vector3 _startPosition;
    private CanvasGroup _canvasGroup;
    private WaitForSeconds _delay = new WaitForSeconds(0.3f);

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        _startPosition = _pointsText.transform.position;
    }

    private void OnEnable()
    {
        _points.CollectedWithText += ShowPanel;
    }

    private void OnDisable()
    {
        _points.CollectedWithText -= ShowPanel;
    }

    private void ShowPanel(string typePoint, int points)
    {
        _typeText.text = typePoint;
        _pointsText.text = _plus + points.ToString();
        _canvasGroup.DOFade(_endSize, _duration);
        var tweener = _rectTransform.DOScale(_endSize, _duration);
        tweener.OnComplete(HidePanel);
    }

    private void HidePanel()
    {
        if(enabled == true)
            StartCoroutine(DelayAndHide());
    }

    private IEnumerator DelayAndHide()
    {
        yield return _delay;
        _canvasGroup.DOFade(0, _duration);
        
        _rectTransform.DOScale(_startSize, _duration);
        _pointsText.transform.position = _startPosition;
    }
}
