using System;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{
    [SerializeField] private Transform _FireTransform;
    [SerializeField] private float ThrowPower = 15f;
    [SerializeField] private int _chance = 5;

    public event Action<int> OnBombCreated;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        if (Input.GetMouseButtonDown(2))
        {
            if (_chance <= 0) return;

            GameObject bomb = BombFactory.Instance.MakeBomb(_FireTransform.transform.position);
            if (bomb == null) return;
            bomb.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            bomb.GetComponent<Rigidbody>().AddForce(_camera.transform.forward * ThrowPower, ForceMode.Impulse);
            Debug.DrawRay(_camera.transform.position,

              _camera.transform.forward * 3f,
              Color.red, 2f);
            _chance--;
            OnBombCreated?.Invoke(_chance);
        }
    }
}
