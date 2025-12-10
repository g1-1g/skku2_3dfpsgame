using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private ParticleSystem _hitEffectPrefab;
    private void Update()
    {
        // 1. 마우스 왼쪽 버튼이 눌린다면
        if (Input.GetMouseButtonDown(0))
        {
            // 2. Ray를 생성하고 발사할 위치, 방향, 거리를 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f, Color.red, 2f);
            // 3. RayCasetHit(충돌한 대상의 정보)를 저장할 변수를 생성한다.
            RaycastHit hitInfo = new RaycastHit();

            // 4. 발사하고
            bool isHit  = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                //5. 충돌했다면... 피격 이펙트 표시
                Debug.Log($"Hit : {hitInfo.transform.name} ");
                ParticleSystem hitEffect = Instantiate(_hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                hitEffect.Play();
            }
        }

    }

}
