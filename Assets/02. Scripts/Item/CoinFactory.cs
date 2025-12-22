using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CoinFactory : MonoBehaviour
{
    [SerializeField] private GameObject[] _coinPrefabs;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 50;

    private ObjectPool<GameObject> _coinPool;

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

}
