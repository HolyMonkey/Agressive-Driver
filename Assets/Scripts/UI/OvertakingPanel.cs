using UnityEngine;
using TMPro;

public class OvertakingPanel : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _overtakingText;
    [SerializeField] private TMP_Text _comboText;
    [SerializeField] private float _comboTime;

    private float _startComboTime;
    private int _counter;
    private bool _isPanelEnable;

    private void OnEnable()
    {
        _player.PlayerOvertook += OnPlayerOvertook;
    }

    private void OnDisable()
    {
        _player.PlayerOvertook -= OnPlayerOvertook;
    }

    private void Awake()
    {
        _startComboTime = _comboTime;
        _isPanelEnable = false;
    }

    private void Update()
    {
        if (_comboTime <= 0f)
        {
            _counter = 0;


            if (_isPanelEnable)
            {
                _animator.SetTrigger("Disable");
                _isPanelEnable = false;
            }
            
        }
        else
        {
            _comboTime -= Time.deltaTime;
        }
    }

    private void OnPlayerOvertook()
    {
        if (_isPanelEnable == false)
        {
            _animator.SetTrigger("Enable");

            _isPanelEnable = true;
        }

        _comboTime = _startComboTime;
        _counter++;

        _comboText.text = "combo x" + _counter;

        if(_counter > 1)
            _animator.SetTrigger("Counter");
    }
}
