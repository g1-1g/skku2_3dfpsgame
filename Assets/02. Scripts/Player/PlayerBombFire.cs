using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{
    [SerializeField] private Transform _FireTransform;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private float ThrowPower = 15f;


    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            GameObject bomb = BombFactory.Instance.CreateBomb(_FireTransform.position);
            if (bomb == null) return;
            bomb.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
        }
    }
}
