using System;
using UnityEngine;
using UnityEngine.Pool;

public class BombFactory : MonoBehaviour
{
    static BombFactory _instance;
    static public BombFactory Instance => _instance;

    public GameObject BombPrefab;
    private GameObject[] _bombObjectPool;
    private int _poolSize = 5;


    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        PoolInit();
    }

    public void Start()
    {

    }

    public void PoolInit()
    {
        _bombObjectPool = new GameObject[_poolSize];
        for ( int i = 0; i < _poolSize; i++ )
        {
            GameObject bomb = Instantiate(BombPrefab, transform);
            _bombObjectPool[i] = bomb;
            bomb.SetActive(false);
        }
    }

    public GameObject MakeBomb(Vector3 position)
    {
        for (int i = 0; i < _poolSize; ++i)
        {
            GameObject bomb = _bombObjectPool[i];
            if (bomb == null) continue;
            if ( _bombObjectPool[i].activeInHierarchy == false)
            {
                bomb.transform.position = position;
                bomb.SetActive(true);

                return bomb;
            } 
        }
        Debug.Log("사용 가능한 폭탄이 없습니다.");
        return null;
    }
}
