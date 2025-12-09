using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private Transform _FireTransform;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private float ThrowPower = 15f;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Bomb bomb  = Instantiate (_bombPrefab, _FireTransform.position, Quaternion.identity);
            bomb.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
        }
    }
}
