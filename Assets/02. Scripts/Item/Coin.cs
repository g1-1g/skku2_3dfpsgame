using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinFactory _factory;
    private float _lifeTime = 10f;
    private float _timer = 0;

    private float _dropBound = 5;
    private Rigidbody _rb;

    private GameObject _player;
    private bool _isFollowing = false;
    private float _followingSpeed = 10;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void OnEnable()
    {
        _timer = 0f;
        _isFollowing = false;
    }

    public void SetPool(CoinFactory factory)
    {
        _factory = factory;
    }

    public void Launch(Vector3 direction)
    {
        _rb.AddForce(direction * _dropBound);
        _rb.AddTorque(Random.insideUnitSphere * 20f);
    }

    void Update()
    {
        transform.Rotate(Vector3.up, 180f * Time.deltaTime);

        if (_isFollowing)
        {
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            transform.Translate(direction * Time.deltaTime * _followingSpeed, Space.World);
        }

        _timer += Time.deltaTime;
        if (_timer > _lifeTime)
        {
            ReturnPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        _player = other.gameObject;
        _isFollowing = true;
        _rb.Sleep();    
    }

    public void ReturnPool()
    {
        if (_factory != null)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.WakeUp();
            _factory.ReturnCoin(gameObject);
        }
    } 
}
