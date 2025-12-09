using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float _moveSpeed = 5f;

    public float Gravity = -9.81f;
    public float JumpPower = 5f;

    private CharacterController _characterController;
    private float _yVelocity = 0f;
    [SerializeField] private float _hp = 100f;
    [SerializeField] private float _stamina = 100f;

    private bool isDashing = false;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        _yVelocity += Gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && _characterController.isGrounded)
        {
            _yVelocity = JumpPower;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(x, 0, z).normalized;
      
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = _yVelocity;

        _characterController.Move(direction * _moveSpeed * Time.deltaTime);

        StaminaChange();
    }

    void StaminaChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _stamina >= 0)
        {
            _moveSpeed = 10f;
            isDashing = true;

            StartCoroutine(StaminaDecrease());
            return;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed = 5f;
            isDashing = false;
        }
        StartCoroutine(StaminaIncrease());
    }

    IEnumerator StaminaDecrease()
    {
        while (isDashing && _stamina > 0)
        {
            _stamina--;
            Debug.Log(_stamina);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator StaminaIncrease()
    {
        while (!isDashing && _stamina < 100)
        {
            _stamina = MathF.Max(_stamina + 1, 100);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
