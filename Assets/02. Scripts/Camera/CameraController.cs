using UnityEngine;

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

        transform.position = basePosition + shakePosition;

        Quaternion baseRot = Quaternion.Euler(_rotate.BaseRotation);
        Quaternion shakeRot = Quaternion.Euler(_shake.ShakeRotation);

        transform.localRotation = baseRot * shakeRot;
    }

}
