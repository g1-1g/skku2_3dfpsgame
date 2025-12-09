using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _hp = 100f;
    [SerializeField] private float _stamina = 100f;
    public float Stamina => _stamina;

    float _moveSpeed = 5f;
    private float _gravity = -9.81f * 2f;

    public float JumpPower = 10f;
    private float _doubleJumpStaminaCost = 20f;
    private bool _canDoubleJump = true;


    private CharacterController _characterController;
    private float _yVelocity = 0f;

    

    private float _staminaDecreaseRate = 0.05f;
    private float _staminaIncreaseRate = 0.1f;
    private bool _isDashing = false;
    private bool _isIncreasingStamina = false;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        _yVelocity += _gravity * Time.deltaTime;

        Jump();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(x, 0, z).normalized;
      
        direction = Camera.main.transform.TransformDirection(direction) * _moveSpeed;
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
                _yVelocity = JumpPower;
            }  
            else if (_canDoubleJump && _stamina >= _doubleJumpStaminaCost)
            {
                _canDoubleJump = false;
                _stamina -= _doubleJumpStaminaCost;
                _yVelocity = JumpPower;
            }
        }

    }

    IEnumerator DoubleJumpStaminaDecrease(float cost)
    {
        float applyCost = 0;
        Debug.Log(_stamina);
        while (applyCost < cost && _stamina > 0)
        {
            applyCost++;
           
            _stamina = MathF.Max(_stamina - 1, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }

    //데쉬
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _stamina >= 0)
        {
            _moveSpeed = 10f;
            _isDashing = true;

            StartCoroutine(StaminaDecrease());
            return;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed = 5f;
            _isDashing = false;
        }
    }

    IEnumerator StaminaDecrease()
    {
        while (_isDashing && _stamina > 0)
        {
            _stamina = Mathf.Max(_stamina - 1, 0);
            yield return new WaitForSeconds(_staminaDecreaseRate);
        }
    }

    IEnumerator StaminaIncrease()
    {
        _isIncreasingStamina = true;
        while (!_isDashing && _stamina < 100)
        {
            _stamina = MathF.Min(_stamina + 1, 100);
            yield return new WaitForSeconds(_staminaIncreaseRate);
        }
        _isIncreasingStamina = false;
    }
}
