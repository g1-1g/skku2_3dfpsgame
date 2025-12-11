using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject _explosionEffectPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_explosionEffectPrefab,transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }
}
