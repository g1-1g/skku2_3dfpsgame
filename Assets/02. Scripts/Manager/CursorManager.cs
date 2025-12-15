using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public bool _isLock;
    void Awake()
    {
        
    }

    private void Update()
    {
        if (_isLock)
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
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
}