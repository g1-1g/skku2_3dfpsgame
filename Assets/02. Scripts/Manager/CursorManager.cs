using System;
using UnityEngine;
using static UnityEditor.SceneView;

public class CursorManager : MonoBehaviour
{
    public bool _isLock;
    public bool _recentLockState;
    public ECameraMode _cameraMode;

    private void Start()
    {
        CameraManager.Instance.OnCameraModeChanged += CameraModeChanged;
        GameManager.Instance.OnGameStateChanged += GameStateChanged;
        if (_isLock) LockCursor();
    }

    private void GameStateChanged(EGameState state)
    {
        if (state == EGameState.Pause)
        {
            _recentLockState = _isLock;
            SetCursorLock(false);
        }
        if (state == EGameState.Playing)
        {
            CameraModeChanged(_cameraMode);
        }
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
        _cameraMode = cameraMode;
    }

    public void SetCursorLock(bool lockCursor)
    {
        if (_isLock == lockCursor) return;

        _isLock = lockCursor;
        if (lockCursor) LockCursor();
        else UnlockCursor();
    }
}