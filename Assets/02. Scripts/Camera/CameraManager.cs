using System;
using UnityEngine;
using static UnityEditor.SceneView;

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

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetCameraMode(ECameraMode cameraMode)
    {
        CameraMode = cameraMode;
        OnCameraModeChanged?.Invoke(CameraMode);
    }
}
