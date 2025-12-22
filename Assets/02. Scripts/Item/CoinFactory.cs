using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;
using UnityEngine.Pool;

public class CoinFactory : MonoBehaviour
{
    public static CoinFactory Instance { get; private set; }

    [SerializeField] private GameObject[] _coinPrefabs;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 50;

    [SerializeField] private float _burstForce = 20;

    private ObjectPool<GameObject> _coinPool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        _coinPool = new ObjectPool<GameObject>(
            createFunc: CreateCoin, //코인 생성
            actionOnGet: OnGetCoin, //풀에서 꺼냄
            actionOnRelease: OnReleaseCoin, //풀에 반환
            actionOnDestroy: OnDestroyCoin, //풀에서 제거 (maxSize 초과시)
            collectionCheck: true, //중복반환체크
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize
        );

        // 초기 풀 워밍업 (선택사항)
        List<GameObject> temp = new List<GameObject>();
        for (int i = 0; i < _defaultCapacity; i++)
        {
            temp.Add(_coinPool.Get());
        }
        foreach(var coin in temp)
        {
            _coinPool.Release(coin);
        }
    }
    private GameObject CreateCoin()
    {
        GameObject coin = Instantiate(_coinPrefabs[UnityEngine.Random.Range(0, _coinPrefabs.Length)]);
        coin.transform.SetParent(transform);

        // Coin 컴포넌트에 풀 참조 전달
        var coinComponent = coin.GetComponent<Coin>();
        if (coinComponent != null)
        {
            coinComponent.SetPool(this);
        }

        coin.transform.SetParent(gameObject.transform);
        return coin;
    }

    private void OnGetCoin(GameObject coin)
    {
        coin.SetActive(true);
    }

    private void OnReleaseCoin(GameObject coin)
    {
        coin.SetActive(false);
        coin.transform.position = Vector3.zero;
        coin.transform.rotation = Quaternion.identity;
    }
    

    private void OnDestroyCoin(GameObject coin)
    {
        Destroy(coin);
    }

    public GameObject GetCoin()
    {
        return _coinPool.Get();
    }

    public void ReturnCoin(GameObject coin)
    {
        _coinPool.Release(coin);
    }

    public void SpawnCoinAt(Vector3 position)
    {
        GameObject coin = _coinPool.Get();
        coin.transform.position = position;
    }

    public void SpawnCoinBundle(Vector3 position, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Coin coin = _coinPool.Get().GetComponent<Coin>();
            coin.transform.position = position;

            // 원형으로 랜덤 방향
            Vector3 randomDir = Random.insideUnitSphere.normalized;
            randomDir.y = Mathf.Abs(randomDir.y) * _burstForce; // 위쪽으로만

            coin.Launch(randomDir);
        }
    }

}
