using System.Collections;
using System.Threading;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject _explosionEffectPrefab;

    public float ExplosionRadius = 2;

    private Collider[] _colliders = new Collider[10];

    private float _destroyDelay = 1f; 
    private bool _isExploded = false;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _damage = 100;
    private void OnCollisionEnter(Collision collision)
    {
        if (_isExploded) return;
        Instantiate(_explosionEffectPrefab,transform.position, Quaternion.identity);
        _isExploded = true;
        Attack();

        StartCoroutine(BombDestroy());
    }

    private IEnumerator BombDestroy()
    {
        yield return new WaitForSeconds(_destroyDelay);
        gameObject.SetActive(false);
        _isExploded = false;
    }

 
    private void Attack()
    {
        int HitCount = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, _colliders, _layerMask);
        for (int i = 0; i < HitCount; i++)
        {
            if (_colliders[i].TryGetComponent<Monster>(out Monster monster))
            {
                float distance = Mathf.Max(1f, Vector3.Distance(transform.position, monster.transform.position));

                float finalDamage = _damage / distance;

                monster.TryTakeDamage(finalDamage, Vector3.up);
            }
            if (_colliders[i].TryGetComponent<Drum>(out Drum drum))
            {
                float distance = Mathf.Max(1f, Vector3.Distance(transform.position, drum.transform.position));

                float finalDamage = _damage / distance;

                drum.TryTakeDamage(finalDamage, Vector3.up);
            }
        }
    }
}
