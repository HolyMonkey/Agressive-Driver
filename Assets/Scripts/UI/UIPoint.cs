using System.Collections;
using UnityEngine;
using TMPro;

public class UIPoint : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI pointsText;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    public void SetText(string typePoint, int point)
    {
        typeText.text = typePoint;
        pointsText.text = "+" + point.ToString();
    }
}