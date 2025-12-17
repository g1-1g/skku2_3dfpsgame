using UnityEngine;
using static UnityEditor.SceneView;

public class CursorManager : MonoBehaviour
{
    public bool _isLock;

    void Awake()
    {
        
    }

    private void Start()
    {
        CameraManager.Instance.OnCameraModeChanged += CameraModeChanged;
        if (_isLock) LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CameraModeChanged(ECameraMode cameraMode)
    {
        SetCursorLock(cameraMode != ECameraMode.TopView);
    }

    public void SetCursorLock(bool lockCursor)
    {
        if (_isLock == lockCursor) return;

        _isLock = lockCursor;
        if (lockCursor) LockCursor();
        else UnlockCursor();
    }
}