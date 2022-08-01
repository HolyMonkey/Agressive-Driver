using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;

public class Points : MonoBehaviour
{
    public int BoardingCount => _boardingCount;
    public int NearMissCount => _nearMissCount;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerChanger _playerChanger;
    [SerializeField] private NearMissChecker _checker;
    [SerializeField] private float _minSpeedForNearMiss = 30;

    private List<Enemy> _boarding = new List<Enemy>();
    private List<Enemy> _nearMiss = new List<Enemy>();
    private bool _isCollision = false;
    private Coroutine _calculatePoints;

    private int _boardingCount = 0;
    private int _nearMissCount = 0;

    private StringBuilder _brutalText = new StringBuilder("Брутально 50");
    private StringBuilder _nearMissText = new StringBuilder("Близкое столкновение 100");

    public event Action<int> Collected;
    public event Action<string, int> CollectedWithText;

    private void OnEnable()
    {
        _player.Boarding += OnBoarding;
        _player.PlayerHited += OnCollision;
        _checker.NearMissed += NearMiss;
        _playerChanger.PlayerChanged += OnPlayerChanged;
        _playerChanger.NearMissCheckerChanged += OnNearMissCheckerChanged;
    }

    private void OnDisable()
    {
        _player.Boarding -= OnBoarding;
        _player.PlayerHited -= OnCollision;
        _checker.NearMissed -= NearMiss;
        _playerChanger.PlayerChanged -= OnPlayerChanged;
        _playerChanger.NearMissCheckerChanged -= OnNearMissCheckerChanged;
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif

        // Always wait for it if invoking something immediately in the first scene.
        yield return YandexGamesSdk.WaitForInitialization();
        
        if (YandexGamesSdk.Environment.i18n.tld == "com")
        {
            _brutalText = new StringBuilder("Brutal 50");
            _nearMissText = new StringBuilder("Near miss 100");
        }
        else if(YandexGamesSdk.Environment.i18n.tld == "com.tr")
        {
            _brutalText = new StringBuilder("ACIMASIZ 50");
            _nearMissText = new StringBuilder("YAKIN BAYAN 100");
        }
        else if(YandexGamesSdk.Environment.i18n.tld == "ru")
        {
            _brutalText = new StringBuilder("Брутально 50");
            _nearMissText = new StringBuilder("Близкое столкновение 100");
        }
    }

    private void OnNearMissCheckerChanged(NearMissChecker newNearMissChecker)
    {
        _checker.NearMissed -= NearMiss;
        _checker = newNearMissChecker;
        _checker.NearMissed += NearMiss;
    }

    private void OnPlayerChanged(Player newPlayer)
    {
        _player.Boarding -= OnBoarding;
        _player.PlayerHited -= OnCollision;
        _player = newPlayer;
        _player.Boarding += OnBoarding;
        _player.PlayerHited += OnCollision;
    }

    private void OnCollision(Vector3 vector)
    {
        _isCollision = true;
        StartCalculate();
    }

    private void NearMiss(Enemy enemy)
    {
        if (_nearMiss.Contains(enemy) == false)
        {
            _nearMiss.Add(enemy);
            StartCalculate();
        }
    }

    private void OnBoarding(Enemy enemy)
    {
        if (_boarding.Contains(enemy) == false)
        {
            _boarding.Add(enemy);
            StartCalculate();
        }
    }

    private void StartCalculate()
    {
        if (_calculatePoints != null)
            StopCoroutine(_calculatePoints);
        _calculatePoints = StartCoroutine(CalculatePoints());
    }

    private IEnumerator CalculatePoints()
    {
        yield return new WaitForSeconds(0.5f);
        var point = 0;
        var typePoint = String.Empty;
        if (_player.Health > 0)
        {
            if (_boarding.Count > 0)
            {
                point = _boarding.Count * 50;
                typePoint = _brutalText.ToString();
                _boardingCount += _boarding.Count;
            }
            else if (_isCollision == false && _nearMiss.Count > 0 && _playerChanger.CurrentPlayerMover.Speed > _minSpeedForNearMiss)
            {            
                point = _nearMiss.Count * 100;
                typePoint = _nearMissText.ToString();
                _nearMissCount += _nearMiss.Count;
            }
            CollectedWithText?.Invoke(typePoint, point);
            Collected?.Invoke(point);
            _boarding.Clear();
            _nearMiss.Clear();
            _isCollision = false;
        }
    }
}
