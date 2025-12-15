using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(DrumStats))]
[RequireComponent(typeof(Rigidbody))]
public class Drum : MonoBehaviour
{

    private DrumStats _drumStats;
    private Rigidbody _rigidbody;
    private float _radius = 5;
    [SerializeField] private LayerMask _layerMask;
    private Collider[] _colliders = new Collider[20];

    [SerializeField] private float _knockBackPower = 10;
    [SerializeField] private GameObject _ExplosionPrefabs;

    void Start()
    {
        _drumStats = GetComponent<DrumStats>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TryTakeDamage(float Damage, Vector3 direction)
    {
        if (_drumStats.IsExploded) return;
        _rigidbody.AddForce(direction * _knockBackPower);
        _drumStats.Health.Decrease(Damage);

        if (_drumStats.Health.Value <= 0)
        {
            _drumStats.IsExploded = true;
            Explosion();
            return;
        }
    }

    void Explosion()
    {
        Instantiate(_ExplosionPrefabs, transform.position, Quaternion.identity);
        Attack();
        _rigidbody.AddForce(Vector3.up *_drumStats.Power.Value);
        _rigidbody.AddTorque(Random.insideUnitSphere * _knockBackPower);


        Destroy(gameObject, 3f);
    }

    private void Attack()
    {
        int HitCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, _colliders, _layerMask);
        for (int i = 0; i < HitCount; i++)
        {
            if (_colliders[i].TryGetComponent<Monster>(out Monster monster))
            {
                float distance = Mathf.Max(1f, Vector3.Distance(transform.position, monster.transform.position));

                //float finalDamage = Mathf.Max(_drumStats.Damage.Value / distance, 20f);

                monster.TryTakeDamage(_drumStats.Damage.Value, Vector3.up);
            }
            if (_colliders[i].TryGetComponent<Drum>(out Drum drum))
            {
                float distance = Mathf.Max(1f, Vector3.Distance(transform.position, drum.transform.position));

                drum.TryTakeDamage(_drumStats.Damage.Value, Vector3.up);
            }
        }
    }
}
