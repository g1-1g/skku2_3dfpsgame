using System;
using UnityEngine;

public class BombFactory : MonoBehaviour
{
    static BombFactory _instance;
    static public BombFactory Instance => _instance;

    public event Action<int> OnBombCreated;

    private int _remainBombCount = 5;
    private int _maxBombs = 5;

    public GameObject bombPrefab;
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
    }

    public void Start()
    {
        _remainBombCount = _maxBombs;
    }

    public GameObject CreateBomb(Vector3 position)
    {
        if (_remainBombCount > 0)
        {
            _remainBombCount--;
            OnBombCreated?.Invoke(_remainBombCount);
            return Instantiate(bombPrefab, position, Quaternion.identity);
        }
        else
        {
            Debug.Log("폭탄을 모두 사용하였습니다.");
            return null;
        }
    }
}
