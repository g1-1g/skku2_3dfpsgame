using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public Vector3 ShakeOffset { get; private set; }
    public Vector3 ShakeRotation { get; private set; }

    [SerializeField] private PlayerGunFire _gunFire;

    [SerializeField] private float _power = 0.3f;
    [SerializeField] private float _rotationPower = 2f;
    [SerializeField] private float _duration = 1.0f;

    public void Start()
    {
        _gunFire.Shoot += Shake;
    }
    public void Shake(PlayerGunFire.Gun Gun)
    {
        DOTween.Kill("CameraShake");
        ShakeOffset = Vector3.zero;

        DOTween.Punch(
            () => ShakeOffset,
            x => ShakeOffset = x,
            -transform.forward * _power,
            _duration,
            5,
            1
        ).SetId("CameraShake");

        // 회전 반동 (위로 튐 + 미세 좌우)
        Vector3 recoilRot = new Vector3(
            Random.Range(Gun.Recoil * 0.8f, Gun.Recoil * 1.2f), // pitch ↑
            Random.Range(-Gun.Recoil * 0.2f, Gun.Recoil * 0.2f), // yaw 살짝 흔들기
            0
        );

        DOTween.Punch(
            () => ShakeRotation,
            x => ShakeRotation = x,
            recoilRot,
            _duration,
            6,
            0.5f
        ).SetId("CameraShake");
    }

    private void OnDestroy()
    {
        if (_gunFire != null) _gunFire.Shoot -= Shake;
    }
}
