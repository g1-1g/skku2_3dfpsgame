using System;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{
    [SerializeField] private Transform _FireTransform;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private float ThrowPower = 15f;
    [SerializeField] private int _chance = 5;

    public event Action<int> OnBombCreated;
    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (_chance <= 0) return;

            GameObject bomb = BombFactory.Instance.MakeBomb(_FireTransform.position);
            if (bomb == null) return;
            bomb.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
            _chance--;
            OnBombCreated?.Invoke(_chance);
        }
    }
}
