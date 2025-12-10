using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{

    private CameraFollow _follow;
    private CameraShake _shake;
    private CameraRotate _rotate;

    private void Start()
    {
        _follow = GetComponent<CameraFollow>();
        _shake = GetComponent<CameraShake>();
        _rotate = GetComponent<CameraRotate>();
    }
    private void LateUpdate()
    {
        Vector3 basePosition = _follow.BasePosition;
        Vector3 shakePosition = _shake.ShakeOffset;
        Vector3 shakeRotation = _shake.ShakeRotation;

        transform.position = basePosition + shakePosition;
        _rotate.SetRecoil(shakeRotation.x, shakeRotation.y);
    }

}
