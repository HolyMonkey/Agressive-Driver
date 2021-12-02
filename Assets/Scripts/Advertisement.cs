using System.Collections;
using UnityEngine;
using YandexGames;

public class Advertisement : MonoBehaviour
{
    private bool _isTapedReviveButton = false;
    private Player _player;
    private PlayerEffects _playerEffects;
    private GameOver _gameOver;
    private PlayerMover _playerMover;

    public bool IsTapedReviveButton => _isTapedReviveButton;

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif

        // Always wait for it if invoking something immediately in the first scene.
        yield return YandexGamesSdk.WaitForInitialization();

        // Avoid unexpected authorization window popup that will freak out the user.
        if (PlayerAccount.IsAuthorized)
        {
            // Authenticate silently without requesting photo and real name permissions.
            PlayerAccount.Authenticate(false);
        }
    }

    private void EditIsTapedReviveButtonToTrue()
    {
        _isTapedReviveButton = true;
    }

    private void OnShowVideoButtonClick()
    {
        VideoAd.Show();
    }

    public void GiveSecondChance()
    {
        if (_isTapedReviveButton == false)
        {
            _gameOver = GetComponent<GameOver>();
            _player = FindObjectOfType<Player>();
            _playerEffects = _player.GetComponent<PlayerEffects>();
            _playerMover = _player.GetComponent<PlayerMover>();

            OnShowVideoButtonClick();
            _playerMover.SetIsPlayerDied();
            _playerEffects.OnPlayerDelayRevived();
            _gameOver.SetActiveGameOverPanel();
            _player.GetSecondChance();
            EditIsTapedReviveButtonToTrue();
        }
    }

    public void EditIsTapedReviveButtonToFalse()
    {
        _isTapedReviveButton = false;
    }
}
