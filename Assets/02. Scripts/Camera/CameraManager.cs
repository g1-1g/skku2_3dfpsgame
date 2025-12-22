using System;
using UnityEngine;
using static UnityEditor.SceneView;
using DG.Tweening;

public enum ECameraMode
{
    FirstPerson,
    ThirdPerson,
    TopView,
}
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public ECameraMode CameraMode { get; private set; } = ECameraMode.FirstPerson;

    public event Action<ECameraMode> OnCameraModeChanged;

    public Camera Camera { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Camera = GetComponent<Camera>();
    }

    public void SetCameraMode(ECameraMode cameraMode)
    {
        CameraMode = cameraMode;
        OnCameraModeChanged?.Invoke(CameraMode);
    }

    public void CameraZoomIn(float inSize)
    {
        Camera.DOKill();
        float value = Camera.fieldOfView - inSize;
        Camera.DOFieldOfView(value, 0.5f);
    }

    public void CameraZoomOut(float outSize)
    {
        float value = Camera.fieldOfView + outSize;
        //Camera.DOFieldOfView(value, 1);
        Camera.fieldOfView = value;
    }
}
