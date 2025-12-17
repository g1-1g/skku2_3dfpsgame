using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [Serializable]
    public class MoveConfig
    {
        public float Gravity = -9.81f*2;
        public float _doubleJumpStaminaCost = 20f;
        public float _staminaDecreaseRate = 0.05f;
        public float _staminaIncreaseRate = 0.1f;
    }

    [SerializeField] private MoveConfig _moveConfig;

    private PlayerStats _stats;
   
    private bool _canDoubleJump = true;

    private CharacterController _characterController;
    private float _yVelocity = 0f;

    private bool _isDashing = false;
    private bool _isIncreasingStamina = false;

    private float _speed;

    public event Action<float> StaminaUpdate;

    private ECameraMode _cameraMode;
    private EGameState _gameState;

    void Start()
    {
        
        _characterController = GetComponent<CharacterController>();
        _stats = GetComponent<PlayerStats>();
        _speed = _stats.MoveSpeed.Value;
        CameraManager.Instance.OnCameraModeChanged += CameraModeChanged;
        _cameraMode = CameraManager.Instance.CameraMode;

        GameManager.Instance.OnGameStateChanged += GameStateChanged;
        _gameState = GameManager.Instance.State;
    }

    private void GameStateChanged(EGameState state)
    {
        _gameState = state;
    }
    private void CameraModeChanged(ECameraMode cameraMode)
    {
        _cameraMode = cameraMode;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameState != EGameState.Playing) return;
        _yVelocity += _moveConfig.Gravity * Time.deltaTime;

        Jump();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(x, 0, z).normalized;
      
        direction = transform.TransformDirection(direction) * _speed;
        direction.y = _yVelocity;

        

        Dash();
        if (!_isIncreasingStamina) 
        {
            StartCoroutine(StaminaIncrease());
        }

        if (_cameraMode == ECameraMode.TopView) return;
        _characterController.Move(direction * Time.deltaTime);
    }

    //점프
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (_characterController.isGrounded)
            {
                _canDoubleJump = true;
                _yVelocity = _stats.JumpPower.Value;
            }  
            else if (_canDoubleJump && _stats.Stamina.Value >= _moveConfig._doubleJumpStaminaCost)
            {
                _canDoubleJump = false;
                _stats.Stamina.Decrease(_moveConfig._doubleJumpStaminaCost);
                StaminaUpdate?.Invoke(_stats.Stamina.Value);
                _yVelocity = _stats.JumpPower.Value;
            }
        }

    }

    //데쉬
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _stats.Stamina.Value >= 0)
        {
            _speed = _stats.RunSpeed.Value;
            _isDashing = true;

            StartCoroutine(StaminaDecrease());
            return;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _stats.MoveSpeed.Value;
            _isDashing = false;
        }
    }

    IEnumerator StaminaDecrease()
    {
        while (_isDashing && _stats.Stamina.Value > 0)
        {
            _stats.Stamina.Decrease(1);
            StaminaUpdate?.Invoke(_stats.Stamina.Value);
            yield return new WaitForSeconds(_moveConfig._staminaDecreaseRate);
        }
    }

    IEnumerator StaminaIncrease()
    {
        _isIncreasingStamina = true;
        while (!_isDashing && _stats.Stamina.Value < 100)
        {
            _stats.Stamina.Increase(1);
            StaminaUpdate?.Invoke(_stats.Stamina.Value);
            yield return new WaitForSeconds(_moveConfig._staminaIncreaseRate);
        }
        _isIncreasingStamina = false;
    }

    private void OnDestroy()
    {
        if (CameraManager.Instance != null) CameraManager.Instance.OnCameraModeChanged -= CameraModeChanged;
        if (GameManager.Instance != null) GameManager.Instance.OnGameStateChanged -= GameStateChanged;
    }
}
