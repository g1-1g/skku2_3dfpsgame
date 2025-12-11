using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject _explosionEffectPrefab;
    [SerializeField] private float _damage = 40;
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_explosionEffectPrefab,transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TryTakeDamage(_damage, Vector3.up);
        }
    }
}
