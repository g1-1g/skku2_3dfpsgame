using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    private void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 newPosition = Target.position;
            transform.position = newPosition;
        }
    }
}
