using System;
using System.Collections;
using UnityEngine;
using Agava.YandexGames;
using UnityEngine.UIElements;

public class Advertisement : MonoBehaviour
{
    [SerializeField] private GameObject _reviveFailedPanel;
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
    
    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }
    
    private void OnEnable()
    { 
        _adOpened += OnAdOpened; 
        _adRewarded += OnAdRewarded;
        _adClosed += OnAdClosed;
        _adErrorOccured += OnAdErrorOccured;
    }

    private void OnDisable()
    {
       _adOpened += OnAdOpened;
       _adRewarded += OnAdRewarded;
       _adClosed += OnAdClosed;
       _adErrorOccured += OnAdErrorOccured;
       
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif

        yield return YandexGamesSdk.WaitForInitialization();
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
    }

    private void OnAdErrorOccured(string error)
    {
        Debug.Log("OnAdErrorOccured: " + error);
        _reviveFailedPanel.SetActive(true);
        Time.timeScale = 1;
    }
}
