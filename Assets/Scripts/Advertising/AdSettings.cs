using System;
using System.Collections.Generic;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine.UI;

public class AdSettings : MonoBehaviour, IInterstitialAdListener
{
    [SerializeField] private List<Button> _buttons;
    
    private const string AppKey = "667e788d47927e97f5ab08243b24725dbe07fdb083e46220";

    public event Action AdShowned;

    private void OnEnable()
    {
        foreach (var button in _buttons)
        {
            button.onClick.AddListener(ShowAds);
        }
    }

    private void OnDisable()
    {
        foreach (var button in _buttons)
        {
            button.onClick.RemoveListener(ShowAds);
        }
    }

    private void Start()
    {
        int adTypes = Appodeal.INTERSTITIAL;
        Appodeal.initialize(AppKey, adTypes, true);
        Appodeal.setInterstitialCallbacks(this);
    }

    private void ShowAds()
    {
        if (Appodeal.canShow(Appodeal.INTERSTITIAL) == true && Appodeal.isPrecache(Appodeal.INTERSTITIAL) == false)
        {
            Appodeal.show(Appodeal.INTERSTITIAL);
        }
    }
    
    public void onInterstitialLoaded(bool isPrecache)
    {
        Debug.Log("Loaded " + isPrecache);
    }

    public void onInterstitialFailedToLoad()
    {
        Debug.Log("Failed to load");
        Time.timeScale = 0f;
    }

    public void onInterstitialShowFailed()
    {
        Debug.Log("Show failed");
        Time.timeScale = 0f;
    }

    public void onInterstitialShown()
    {
        Debug.Log("Shown");
    }

    public void onInterstitialClosed()
    {
        Debug.Log("Closed");
        AdShowned?.Invoke();
    }

    public void onInterstitialClicked()
    {
        Debug.Log("Clicked");
    }

    public void onInterstitialExpired()
    {
        Debug.Log("Expired");
        Time.timeScale = 0f;
    }
}