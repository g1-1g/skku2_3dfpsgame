using DG.Tweening;
using UnityEngine;
using static UnityEditor.SceneView;

enum ECameraMode
{
    FirstPerson,
    ThirdPerson
}

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Transform ThirdPersonPosition;

    public Vector3 firstPersonOffset;
    public Vector3 thirdPersonOffset;

    private Vector3 currentOffset;

    public bool _isChanging = false;

    [SerializeField] ECameraMode cameraMode = ECameraMode.FirstPerson;

    private void Start()
    {
        firstPersonOffset = Vector3.zero;
        thirdPersonOffset = ThirdPersonPosition.localPosition;
        currentOffset = firstPersonOffset;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (cameraMode == ECameraMode.FirstPerson)
            {
                DOTween.To(() => currentOffset, x => currentOffset = x,
                    thirdPersonOffset, 1f);
            }
            else
            {
                DOTween.To(() => currentOffset, x => currentOffset = x,
                    firstPersonOffset, 1f);
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
        }
    }
}
