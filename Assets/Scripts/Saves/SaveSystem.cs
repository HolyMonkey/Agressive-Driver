using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using UnityEngine;



public class SaveSystem : MonoBehaviour
{
   [SerializeField] private FinishLevel _finishLevel;
   [SerializeField] private PlayerSelector _playerSelector;
   [SerializeField] private string _leaderboardName = "PlaytestBoard";

   private PlayerData _playerData;
   private string _currentJsonData;
   
   public int Money => _playerData.Money;
   public List<CarData> CarDatas => _playerData.UnlockedCars;

   public event Action<int> MoneyChanged;
   public event Action PlayerDataLoaded;

   private void Awake()
   {
      YandexGamesSdk.CallbackLogging = true;
   }

   private void OnEnable()
   {
      _finishLevel.PointsCounted +=OnPointsCounted;
      _playerSelector.CarPurchased += OnCarPurchased;
   }

   private void OnDisable()
   {
      _finishLevel.PointsCounted -= OnPointsCounted;
      _playerSelector.CarPurchased -= OnCarPurchased;
   }

   private IEnumerator Start()
   {
#if !UNITY_WEBGL || UNITY_EDITOR
      yield break;
#endif
      // Always wait for it if invoking something immediately in the first scene.
      yield return YandexGamesSdk.WaitForInitialization();
      
      Load();
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
      });
   }  
   
   private void OnPointsCounted(int score)
   {
      _playerData.TotalScore += score;
      _playerData.Money += score;
      Agava.YandexGames.Leaderboard.SetScore(_leaderboardName, _playerData.TotalScore);
      MoneyChanged?.Invoke(_playerData.Money);
      Save();
   }

   private void OnCarPurchased(CarData car)
   {
      car.SetBuyed();
      _playerData.Money -= car.Price;
      _playerData.UnlockedCars.Add(car);
      MoneyChanged?.Invoke(_playerData.Money);
      Save();
   }
}
