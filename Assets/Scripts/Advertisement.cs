using System;
using System.Collections;
using UnityEngine;
using Agava.YandexGames;
using Agava.YandexGames.Utility;
using UnityEngine.UIElements;

public class Advertisement : MonoBehaviour
{
    [SerializeField] private GameObject _reviveFailedPanel;
    [SerializeField] private FinishLevel _finishLevel;
    
    private bool _isTapedReviveButton = false;
    private Player _player;
    private PlayerEffects _playerEffects;
    private GameOver _gameOver;
    private PlayerMover _playerMover;
    private  Action _adOpened;
    private  Action _adRewarded;
    private  Action _adClosed;
    private  Action<string> _adErrorOccured;

    public bool IsTapedReviveButton => _isTapedReviveButton;
    
    private IEnumerator Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        yield return YandexGamesSdk.WaitForInitialization();
        
        YandexGamesSdk.CallbackLogging = true;
#endif
        
#if UNITY_EDITOR
        yield return null;
#endif
    }
    
    private void OnEnable()
    { 
        _adOpened += OnAdOpened; 
        _adRewarded += OnAdRewarded;
        _adClosed += OnAdClosed;
        _adErrorOccured += OnAdErrorOccured;
        _finishLevel.Finished += OnLevelFinished;
    }

    private void OnDisable()
    {
       _adOpened -= OnAdOpened;
       _adRewarded -= OnAdRewarded;
       _adClosed -= OnAdClosed;
       _adErrorOccured -= OnAdErrorOccured;
       _finishLevel.Finished -= OnLevelFinished;
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
    }

    private void OnApplicationPause(bool isPaused)
    {
        Silence(isPaused);
    }

    private void Silence(bool silence)
    {
        AudioListener.pause = silence;
        
        if (silence && Time.timeScale != 0)
            Time.timeScale = 0;
        else if (Time.timeScale == 0)
            Time.timeScale = 1;
    }

    private void OnGetDeviceTypeButtonClick()
    {
        Debug.Log($"DeviceType = {Device.Type}");
    }

    private void EditIsTapedReviveButtonToTrue()
    {
        _isTapedReviveButton = true;
    }

    private void GiveSecondChance()
    {                                       
            _gameOver = GetComponent<GameOver>();
            _player = FindObjectOfType<Player>();
            _playerEffects = _player.GetComponent<PlayerEffects>();
            _playerMover = _player.GetComponent<PlayerMover>();
            
            _playerMover.SetIsPlayerDied();
            _playerEffects.OnPlayerDelayRevived();
            _gameOver.SetActiveGameOverPanel();
            _player.GetSecondChance();
            EditIsTapedReviveButtonToTrue();          
    }

    public void OnShowAdButtonClick()
    {
        Debug.Log($"DeviceType = {Device.Type}");
        
        if (_isTapedReviveButton == false)
        {
            VideoAd.Show(_adOpened, _adRewarded, _adClosed, _adErrorOccured);
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
    }

    public void EditIsTapedReviveButtonToFalse()
    {
        _isTapedReviveButton = false;
    }
    
    private void OnAdRewarded()
    {
     Debug.Log("OnAdRewarded"); 
     GiveSecondChance();  
    }

    private void OnAdOpened()
    {
        Debug.Log("OnAdOpened");
    }

    private void OnAdClosed()
    {
        Debug.Log("OnAdClosed");
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    private void OnAdErrorOccured(string error)
    {
        Debug.Log("OnAdErrorOccured: " + error);
        _reviveFailedPanel.SetActive(true);
        Time.timeScale = 1;
    }

    private void OnLevelFinished()
    {
        InterestialAd.Show();
    }
}
