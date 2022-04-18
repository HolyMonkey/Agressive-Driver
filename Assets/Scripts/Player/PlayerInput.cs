using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private ButtonHoldChecker _leftButton;
    [SerializeField] private ButtonHoldChecker _rightButton;
    [SerializeField] private ButtonHoldChecker _accelerationButton;
    
    private bool _accelerationButtonPressed;
    private bool _leftButtonPressed;
    private bool _rightButtonPressed;
    private KeyCode _w;
    private KeyCode _a;
    private KeyCode _d;
    private KeyCode _leftArrow;
    private KeyCode _rightArrow;

    public bool AccelerationButtonPressed => _accelerationButtonPressed;
    public bool LeftButtonPressed => _leftButtonPressed;
    public bool RightButtonPressed => _rightButtonPressed;
    
    private void Start()
    {
        _a = KeyCode.A;
        _d = KeyCode.D;
        _w = KeyCode.W;

        _w = KeyCode.W;
        _leftArrow = KeyCode.LeftArrow;
        _rightArrow = KeyCode.RightArrow;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(_w) || _accelerationButton.IsPressed)
        {
            _accelerationButtonPressed = true;
        }
        else
        {
            _accelerationButtonPressed = false;
        }
        
        if (Input.GetKey(_a) || Input.GetKey(_leftArrow) || _leftButton.IsPressed)
        {
            _leftButtonPressed = true;
        }
        else
        {
            _leftButtonPressed = false;
        }
        
        if (Input.GetKey(_d) || Input.GetKey(_rightArrow) || _rightButton.IsPressed)
        {
            _rightButtonPressed = true;
        }
        else
        {
            _rightButtonPressed = false;
        }
    }
}
