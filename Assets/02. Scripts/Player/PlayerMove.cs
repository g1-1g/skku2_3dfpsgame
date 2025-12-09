using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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

    private MoveConfig _moveConfig;

    [SerializeField] private PlayerStats _stats;
   
    private bool _canDoubleJump = true;

    private CharacterController _characterController;
    private float _yVelocity = 0f;

    private bool _isDashing = false;
    private bool _isIncreasingStamina = false;

    private float _speed;

    void Start()
    {
        _moveConfig = new MoveConfig();
        _speed = _stats.MoveSpeed.Value;
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        _yVelocity += _moveConfig.Gravity * Time.deltaTime;

        Jump();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(x, 0, z).normalized;
      
        direction = Camera.main.transform.TransformDirection(direction) * _stats.MoveSpeed.Value;
        direction.y = _yVelocity;

        _characterController.Move(direction * Time.deltaTime);

        Dash();
        if (!_isIncreasingStamina) 
        {
            StartCoroutine(StaminaIncrease());
        }
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
            yield return new WaitForSeconds(_moveConfig._staminaDecreaseRate);
        }
    }

    IEnumerator StaminaIncrease()
    {
        _isIncreasingStamina = true;
        while (!_isDashing && _stats.Stamina.Value < 100)
        {
            _stats.Stamina.Increase(1);
            yield return new WaitForSeconds(_moveConfig._staminaIncreaseRate);
        }
        _isIncreasingStamina = false;
    }
}
