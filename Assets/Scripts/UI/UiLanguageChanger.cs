using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiLanguageChanger : MonoBehaviour
{
    [SerializeField] private Text[] _trTapToPlayTexts;
    [SerializeField] private TextMeshProUGUI[] _ruTapToPlayTexts;
    [SerializeField] private TextMeshProUGUI[] _enTapToPlayTexts;
    [SerializeField] private GameObject _ruLeaderboardTexts;
    [SerializeField] private GameObject _enLeaderboardTexts;
    [SerializeField] private GameObject _trLeaderboardTexts;
    private IEnumerator Start()
        {
            Debug.Log("UiStartGame");
            Debug.Log(YandexGamesSdk.Environment.i18n.tld);
    #if !UNITY_WEBGL || UNITY_EDITOR
            yield break;
    #endif

            // Always wait for it if invoking something immediately in the first scene.
            yield return YandexGamesSdk.WaitForInitialization();
            
            if (YandexGamesSdk.Environment.i18n.tld == "com")
            {
                SetActiveEnText(true);
                SetActiveTrText(false);
                SetActiveRuText(false);
                _enLeaderboardTexts.gameObject.SetActive(true);
                _ruLeaderboardTexts.gameObject.SetActive(false);
                _trLeaderboardTexts.gameObject.SetActive(false);
            }
            else if(YandexGamesSdk.Environment.i18n.tld == "com.tr")
            {
                SetActiveEnText(false);
                SetActiveTrText(true);
                SetActiveRuText(false);
                _enLeaderboardTexts.gameObject.SetActive(false);
                _ruLeaderboardTexts.gameObject.SetActive(false);
                _trLeaderboardTexts.gameObject.SetActive(true);
            }
            else if(YandexGamesSdk.Environment.i18n.tld == "ru")
            {
                SetActiveEnText(false);
                SetActiveTrText(false);
                SetActiveRuText(true);
                _enLeaderboardTexts.gameObject.SetActive(false);
                _ruLeaderboardTexts.gameObject.SetActive(true);
                _trLeaderboardTexts.gameObject.SetActive(false);
            }
        }

    private void SetActiveTrText(bool value)
    {
        foreach (var text in _trTapToPlayTexts)
        {
            text.gameObject.SetActive(value);
            _trLeaderboardTexts.gameObject.SetActive(value);
        }
    }
    
    private void SetActiveRuText(bool value)
    {
        foreach (var text in _ruTapToPlayTexts)
        {
            text.gameObject.SetActive(value);
            _enLeaderboardTexts.gameObject.SetActive(value);
        }
    }
    
    private void SetActiveEnText(bool value)
    {
        foreach (var text in _enTapToPlayTexts)
        {
            text.gameObject.SetActive(value);
        }
    }
}
