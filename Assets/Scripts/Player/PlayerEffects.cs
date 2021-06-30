using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private ParticleSystem _damageEffect;
    [SerializeField] private ParticleSystem _diedEffect;

    private Player _player;

    private readonly float _needChangeAlphaValue = 0.7f;

    private void OnEnable()
    {
        _player.PlayerHited += OnPlayerHitted;
        _player.PlayerDied += OnPlayerDied;
        _player.HealthChanged += OnHealthChanged;
        _player.DelayRevived += OnPlayerDelayRevived;
    }

    private void OnDisable()
    {
        _player.PlayerHited -= OnPlayerHitted;
        _player.PlayerDied -= OnPlayerDied;
        _player.HealthChanged -= OnHealthChanged;
        _player.DelayRevived -= OnPlayerDelayRevived;
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnPlayerDelayRevived()
    {
        _diedEffect.Stop();
    }
    
    private void OnPlayerHitted(Vector3 hitPoint)
    {
        Vector3 targetPosition = new Vector3(transform.position.x - hitPoint.x, _hitEffect.gameObject.transform.position.y, transform.position.z - hitPoint.z);

        _hitEffect.gameObject.transform.position = targetPosition;
        _hitEffect.Play();
    }

    private void OnPlayerDied()
    {
        _diedEffect.Play();
    }

    private void OnHealthChanged(int currentHealth, int health)
    {
        if((float)currentHealth/health <= _needChangeAlphaValue)
        {
            ChangeAlphaValue(currentHealth, health);
            _damageEffect.Play();
        }

        if(currentHealth <= 0 || currentHealth == health)
        {
            _damageEffect.Stop();
        }
    }

    private void ChangeAlphaValue(int currentHealth, int health)
    {
        float alpha = 1f - (float)currentHealth / health;

        var main = _damageEffect.main;
        main.startColor = new Color(main.startColor.color.r, main.startColor.color.g,
            main.startColor.color.b, alpha);
    }
}
