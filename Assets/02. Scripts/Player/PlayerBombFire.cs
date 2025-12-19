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

    private ECameraMode _cameraMode;
    private EGameState _gameState;

    private Animator _animator;
    private void Start()
    {
        _camera = Camera.main;
        CameraManager.Instance.OnCameraModeChanged += CameraModeChanged;
        _cameraMode = CameraManager.Instance.CameraMode;

        GameManager.Instance.OnGameStateChanged += GameStateChanged;
        _gameState = GameManager.Instance.State;

        _animator = GetComponent<Animator>();
    }

    private void GameStateChanged(EGameState state)
    {
        _gameState = state;
    }
    private void CameraModeChanged(ECameraMode cameraMode)
    {
        _cameraMode = cameraMode;
    }

    private void Update()
    {
        if (_gameState != EGameState.Playing) return;
        if (_cameraMode == ECameraMode.TopView) return;
        if (Input.GetMouseButtonDown(1))
        {
            if (_chance <= 0) return;

            _animator.SetTrigger("Throw");
        }
    }

    private void BombThrow()
    {
        GameObject bomb = BombFactory.Instance.MakeBomb(_FireTransform.transform.position);
        if (bomb == null) return;
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(_camera.transform.forward * ThrowPower, ForceMode.Impulse);
        _chance--;
        OnBombCreated?.Invoke(_chance);
    }

    private void OnDestroy()
    {
        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.OnCameraModeChanged -= CameraModeChanged;
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= GameStateChanged;
        }
    }
}
