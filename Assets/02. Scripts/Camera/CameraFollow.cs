using DG.Tweening;
using UnityEngine;

enum ECameraMode
{
    FirstPerson,
    ThirdPerson
}

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    public Transform ThirdPersonPosition;

    public Vector3 FirstPersonOffset;
    public Vector3 ThirdPersonOffset;

    private Vector3 currentOffset;

    public bool _isChanging = false;

    [SerializeField] ECameraMode cameraMode = ECameraMode.FirstPerson;

    private void Start()
    {
        FirstPersonOffset = Vector3.zero;
        ThirdPersonOffset = ThirdPersonPosition.localPosition;
        currentOffset = FirstPersonOffset;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (cameraMode == ECameraMode.FirstPerson)
            {
                DOTween.Kill(transform);
                DOTween.To(() => currentOffset, x => currentOffset = x,
                    ThirdPersonOffset, 1f);
            }
            else
            {
                DOTween.Kill(transform);
                DOTween.To(() => currentOffset, x => currentOffset = x,
                    FirstPersonOffset, 1f);
            }

            cameraMode = cameraMode == ECameraMode.FirstPerson ? ECameraMode.ThirdPerson : ECameraMode.FirstPerson;

        }
    }

    private void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 rotatedOffset = Target.rotation * currentOffset;
            transform.position = Target.position + rotatedOffset;

            if (cameraMode == ECameraMode.ThirdPerson)
            {
                //transform.LookAt(Target);
            }
            
        }
    }
}
