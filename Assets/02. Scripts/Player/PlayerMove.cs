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
    }
    
}
