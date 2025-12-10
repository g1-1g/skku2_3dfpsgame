using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using static PlayerGunFire;
using static UnityEngine.ParticleSystem;

public class PlayerGunFire : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private ParticleSystem _hitEffectVFX;

    //총알 연사 
    private float _lastShootTime = 0;

    //장전
    private bool _isReloading = false;

    [Serializable]
    public class Gun
    {
        public int _magazineSize = 30;
        public int _currentBullet = 30;
        public int _reserveBullet = 90;
        public float _fireInterval = 1.0f;
        public float _reloadInterval = 1.5f;
    }

    public Gun _basicGun;

    public event Action<Gun> GunUpdate;
    public event Action<Gun> GunReload;

    private void Start()
    {
        GunUpdate?.Invoke(_basicGun);
    }

    private void Update()
    {
        // 1. 마우스 왼쪽 버튼이 눌린다면
        if (Input.GetMouseButton(0))
        {
            if (_isReloading) return;
            GunShooting(_basicGun, _hitEffectVFX);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_isReloading) return;
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
            GunUpdate?.Invoke(gun);
            _lastShootTime = Time.time;
            
        }
    }

    private void Reload(Gun gun)
    {
        if (gun._currentBullet == gun._magazineSize) return;
        if (gun._reserveBullet <= 0)
        {
            Debug.Log("남은 총알이 없습니다.");
            return;
        }
        
        StartCoroutine(Reloading(gun)); 
    }

    IEnumerator Reloading(Gun gun)
    {
        GunReload?.Invoke(gun);
        Debug.Log("총알 장전중");
        _isReloading = true;
        yield return new WaitForSeconds(gun._reloadInterval);
        int need = gun._magazineSize - gun._currentBullet;
        int load = Mathf.Min(need, gun._reserveBullet);

        gun._currentBullet += load;
        gun._reserveBullet -= load;
        Debug.Log("총알 장전완료");
        _isReloading = false;
        GunUpdate?.Invoke(gun);
    }
}
