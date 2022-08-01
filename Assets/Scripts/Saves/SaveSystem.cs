using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveSystem : MonoBehaviour
{
   [SerializeField] private FinishLevel _finishLevel;
   [SerializeField] private PlayerSelector _playerSelector;
   [SerializeField] private string _leaderboardName = "PlaytestBoard";
   [SerializeField] private bool _isInitLevel;

   private PlayerData _playerData;
   private string _currentJsonData;
   private bool _firstLoad = true;
   
   public int Money => _playerData.Money;
   public List<int> UnlockedCars => _playerData.UnlockedCarsId.UnlockedCarsIdArray;

   public event Action<int> MoneyChanged;
   public event Action PlayerDataLoaded;

   private IEnumerator Start()
   {
#if UNITY_WEBGL && !UNITY_EDITOR
        yield return YandexGamesSdk.WaitForInitialization();
        
        YandexGamesSdk.CallbackLogging = true;
#endif
        
#if UNITY_EDITOR
      yield return null;
#endif
      
      Load();
   }

   private void OnEnable()
   {
      if (_isInitLevel == false)
      {
         _finishLevel.PointsCounted += OnPointsCounted;
         _playerSelector.CarPurchased += OnCarPurchased;
      }
   }

   private void OnDisable()
   {
      if (_isInitLevel == false)
      {
         _finishLevel.PointsCounted -= OnPointsCounted;
         _playerSelector.CarPurchased -= OnCarPurchased;
      }
   }
   
   private void Save()
   {
      Agava.YandexGames.Leaderboard.SetScore(_leaderboardName, _playerData.TotalScore);
      _currentJsonData = JsonUtility.ToJson(_playerData);
      PlayerAccount.SetPlayerData(_currentJsonData);
      Load();
   }

   private void Load()
   {
      _playerData = new PlayerData();
      
      PlayerAccount.GetPlayerData((result) =>
      {
         _playerData = JsonUtility.FromJson<PlayerData>(result);
         MoneyChanged?.Invoke(_playerData.Money);
         PlayerDataLoaded?.Invoke();
         TryToLoadLevel();
      });
   }  
   
   private void OnPointsCounted(int score,int levelIndex)
   {
      _playerData.TotalScore += score;
      _playerData.CurrentLevel = levelIndex;
      _playerData.Money += score;
      Agava.YandexGames.Leaderboard.SetScore(_leaderboardName, _playerData.TotalScore);
      MoneyChanged?.Invoke(_playerData.Money);
      Save();
   }

   private void TryToLoadLevel()
   {
      if (_firstLoad)
      {
         if(_playerData.CurrentLevel != SceneManager.GetActiveScene().buildIndex)
            SceneManager.LoadScene(_playerData.CurrentLevel);

         if (_playerData.CurrentLevel == 0)
         {
            _playerData.CurrentLevel = 1;
            Save();
            SceneManager.LoadScene(1);
         }

         _firstLoad = false;
      }
   }
   
   private void OnCarPurchased(int carId, int carPrice)
   {
      _playerData.Money -= carPrice;
      _playerData.UnlockedCarsId.UnlockedCarsIdArray.Add(carId);
      MoneyChanged?.Invoke(_playerData.Money);
      Save();
   }
}
