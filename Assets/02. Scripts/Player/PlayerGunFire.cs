using System;
using System.Collections;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using static PlayerGunFire;
using static UnityEngine.ParticleSystem;

public class PlayerGunFire : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private ParticleSystem _hitEffectVFX;
    private Camera _camera;

    //총알 연사 
    private float _lastShootTime = 0;

    //장전
    private bool _isReloading = false;

    [Serializable]
    public class Gun
    {
        public int MagazineSize = 30;
        public int CurrentBullet = 30;
        public int ReserveBullet = 90;
        public float FireInterval = 0.1f;
        public float ReloadInterval = 1.5f;
        public float Recoil = 2f;
        public int Damage = 20;
    }

    public Gun _basicGun;

    public event Action<Gun> GunUpdate;
    public event Action<Gun> GunReload;
    public event Action<Gun> Shoot;

    private void Start()
    {
        GunUpdate?.Invoke(_basicGun);
        _camera = Camera.main;
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
        if (Time.time > _lastShootTime + gun.FireInterval)
        {
            if (gun.CurrentBullet <= 0)
            {
                Debug.Log("총알을 재장전 하세요");
                return;
            }

            // 2. Ray를 생성하고 발사할 위치, 방향, 거리를 설정한다.
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
            Debug.DrawRay(_camera.transform.position, _camera.transform.forward * 100f, Color.red, 2f);
            // 3. RayCasetHit(충돌한 대상의 정보)를 저장할 변수를 생성한다.
            RaycastHit hitInfo = new RaycastHit();

            // 4. 발사하고
            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
            bool isHit = Physics.Raycast(ray, out hitInfo,100f, layerMask);
            Shoot?.Invoke(gun);
            if (isHit)
            {
                //5. 충돌했다면... 피격 이펙트 표시
                Debug.Log($"Hit : {hitInfo.transform.name} ");

                MonsterMove monster = hitInfo.transform.GetComponent<MonsterMove>();
                if (monster != null) monster.TryTakeDamage(gun.Damage, -hitInfo.normal);
                vfx.transform.position = hitInfo.point;
                vfx.transform.forward = hitInfo.normal;

                vfx.Emit(1);
            }

            gun.CurrentBullet--;
            GunUpdate?.Invoke(gun);
            _lastShootTime = Time.time;
            
        }
    }


    private void Reload(Gun gun)
    {
        if (gun.CurrentBullet == gun.MagazineSize) return;
        if (gun.ReserveBullet <= 0)
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
        yield return new WaitForSeconds(gun.ReloadInterval);
        int need = gun.MagazineSize - gun.CurrentBullet;
        int load = Mathf.Min(need, gun.ReserveBullet);

        gun.CurrentBullet += load;
        gun.ReserveBullet -= load;
        Debug.Log("총알 장전완료");
        _isReloading = false;
        GunUpdate?.Invoke(gun);
    }
}
