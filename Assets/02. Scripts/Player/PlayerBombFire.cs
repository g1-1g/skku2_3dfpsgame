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
    private void Start()
    {
        _camera = Camera.main;
        CameraManager.Instance.OnCameraModeChanged += CameraModeChanged;
        _cameraMode = CameraManager.Instance.CameraMode;
    }

    private void CameraModeChanged(ECameraMode cameraMode)
    {
        _cameraMode = cameraMode;
    }

    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        if (_cameraMode == ECameraMode.TopView) return;
        if (Input.GetMouseButtonDown(1))
        {
            if (_chance <= 0) return;

            GameObject bomb = BombFactory.Instance.MakeBomb(_FireTransform.transform.position);
            if (bomb == null) return;
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(_camera.transform.forward * ThrowPower, ForceMode.Impulse);
            _chance--;
            OnBombCreated?.Invoke(_chance);
        }
    }
    private void OnDestroy()
    {
        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.OnCameraModeChanged -= CameraModeChanged;
        }
    }
}
