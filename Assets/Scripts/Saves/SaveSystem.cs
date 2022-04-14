using System;
using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.Networking;

public class SaveSystem : MonoBehaviour
{
   [SerializeField] private FinishLevel _finishLevel;
   [SerializeField] private string _leaderboardName = "PlaytestBoard";
   
   private PlayerData _playerData;
   private string _currentJsonData;

   private void Awake()
   {
      YandexGamesSdk.CallbackLogging = true;
   }

   private void OnEnable()
   {
      _finishLevel.PointsCounted +=OnPointsCounted;
   }

   private void OnDisable()
   {
      _finishLevel.PointsCounted -= OnPointsCounted;
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
      });
   }
   
   private void OnPointsCounted(int score)
   {
      _playerData.TotalScore += score;
      Agava.YandexGames.Leaderboard.SetScore(_leaderboardName, _playerData.TotalScore);
      Save();
   }
}
