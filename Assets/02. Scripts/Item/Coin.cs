using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinFactory _factory;
    private float _lifeTime = 3f;
    private float _timer = 0;


    void OnEnable()
    {
        _timer = 0f;
    }

    public void SetPool(CoinFactory factory)
    {
        _factory = factory;
    }

    void Update()
    {
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
