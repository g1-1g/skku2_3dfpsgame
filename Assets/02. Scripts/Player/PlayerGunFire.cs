using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerGunFire : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private ParticleSystem _hitEffectVFX;

    //총알 연사 
    private float _lastShootTime = 0;

    [Serializable]
    public class Gun
    {
        public int _maxBullet = 30;
        public int _currentBullet = 30;
        public float _fireInterval = 1.0f;
    }

    public Gun _basicGun;

    private void Start()
    {

    }


    private void Update()
    {
        // 1. 마우스 왼쪽 버튼이 눌린다면
        if (Input.GetMouseButton(0))
        {
            GunShooting(_basicGun, _hitEffectVFX);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload(_basicGun);
        }

    }

    private void GunShooting(Gun gun, ParticleSystem vfx)
    {
        if (Time.time > _lastShootTime + gun._fireInterval)
        {
            if (gun._currentBullet <= 0)
            {
                Debug.Log("총알을 재장전 하세요");
                return;
            }
            
            // 2. Ray를 생성하고 발사할 위치, 방향, 거리를 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f, Color.red, 2f);
            // 3. RayCasetHit(충돌한 대상의 정보)를 저장할 변수를 생성한다.
            RaycastHit hitInfo = new RaycastHit();

            // 4. 발사하고
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                //5. 충돌했다면... 피격 이펙트 표시
                Debug.Log($"Hit : {hitInfo.transform.name} ");

                vfx.transform.position = hitInfo.point;
                vfx.transform.forward = hitInfo.normal;

                vfx.Emit(1);
            }
            gun._currentBullet--;
            _lastShootTime = Time.time;
            
        }
    }

    private void Reload(Gun gun)
    {
        gun._currentBullet = gun._maxBullet;
    }

}
