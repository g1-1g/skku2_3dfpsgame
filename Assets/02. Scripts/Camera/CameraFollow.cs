using DG.Tweening;
using UnityEngine;
using static UnityEditor.SceneView;


public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    public Transform ThirdPersonPosition;

    public Vector3 FirstPersonOffset;
    public Vector3 ThirdPersonOffset;
    public Vector3 TopViewOffset = new Vector3 (0, 10, 0);

    private Vector3 currentOffset;

    public bool _isChanging = false;

    

    public Vector3 BasePosition { get; private set; }

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
            if (CameraManager.Instance.CameraMode == ECameraMode.FirstPerson)
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

            CameraManager.Instance.CameraMode = CameraManager.Instance.CameraMode == ECameraMode.FirstPerson ? ECameraMode.ThirdPerson : ECameraMode.FirstPerson;

        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (CameraManager.Instance.CameraMode == ECameraMode.FirstPerson || CameraManager.Instance.CameraMode == ECameraMode.ThirdPerson)
            {
                DOTween.Kill(transform);
                DOTween.To(() => currentOffset, x => currentOffset = x,
                    TopViewOffset, 1f);
                transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            }
            else
            {
                DOTween.Kill(transform);
                DOTween.To(() => currentOffset, x => currentOffset = x,
                    FirstPersonOffset, 1f);
                transform.localRotation = Quaternion.Euler(Vector3.zero);
            }

            CameraManager.Instance.CameraMode = CameraManager.Instance.CameraMode == ECameraMode.TopView ? ECameraMode.FirstPerson : ECameraMode.TopView;
        }
    }

    private void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 rotatedOffset = Target.rotation * currentOffset;
            BasePosition = Target.position + rotatedOffset;

            if (CameraManager.Instance.CameraMode == ECameraMode.ThirdPerson)
            {
                //transform.LookAt(Target);
            }
            
        }
    }
}
