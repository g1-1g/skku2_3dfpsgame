using JetBrains.Annotations;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    public float RotationSpeed = 5f;

    private float _accumulationX = 0f;
    private float _accumulationY = 0f;

    private void Update()
    {
       
        if (!Input.GetMouseButton(1))
        {
            return;
        }
        //1. 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        //2. 마우스 입력을 누적한다.
        _accumulationX += mouseX * RotationSpeed * Time.deltaTime;
        _accumulationY += mouseY * RotationSpeed * Time.deltaTime;

        //3. ~90도 90도 이상 회전하지 않도록 제한
        _accumulationY = Mathf.Clamp(_accumulationY, -90f, 90f);

        Vector3 rotationDirection = new Vector3(-_accumulationY, _accumulationX, 0f);

        //3. 회전 방향으로 카메라 회전하기
        transform.eulerAngles = rotationDirection;
    }
}

