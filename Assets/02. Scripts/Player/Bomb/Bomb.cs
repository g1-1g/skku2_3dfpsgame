using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject _explosionEffectPrefab;

    public float ExplosionRadius = 2;

    private Collider[] _colliders = new Collider[10];

    [SerializeField] private float _damage = 100;
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_explosionEffectPrefab,transform.position, Quaternion.identity);
        
        Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, _colliders, LayerMask.NameToLayer("Monster"));
        for (int i = 0; i < _colliders.Length; i++)
        {
            Monster monster = _colliders[i].GetComponent<Monster>();
            float distance = Mathf.Min(1f, Vector3.Distance(transform.position, monster.transform.position));

            float finalDamage = _damage / distance; //폭발 원점과 거리에 따라서 데미지를 다르게 준다.

            monster.TryTakeDamage(finalDamage, Vector3.up);
        }

        gameObject.SetActive(false);
    }
}
