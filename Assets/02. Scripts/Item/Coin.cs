using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinFactory _factory;
    private float _lifeTime = 10f;
    private float _timer = 0;

    private float _dropBound = 5;
    [SerializeField] private Rigidbody _rb;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void OnEnable()
    {
        _timer = 0f;
    }

    public void SetPool(CoinFactory factory)
    {
        _factory = factory;
    }

    public void Launch(Vector3 direction)
    {
        _rb.Sleep();
        _rb.WakeUp();
        _rb.AddForce(direction * _dropBound);
        _rb.AddTorque(Random.insideUnitSphere * 20f);
    }

    void Update()
    {
        transform.Rotate(Vector3.up, 180f * Time.deltaTime);

        _timer += Time.deltaTime;
        if (_timer > _lifeTime)
        {
            ReturnPool();
        }
    }

    void ReturnPool()
    {
        if (_factory != null)
        {
            _factory.ReturnCoin(gameObject);
        }
    } 
}
