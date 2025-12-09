using UnityEngine;
<<<<<<< Updated upstream
=======

enum ECameraMode
{
    FirstPerson,
    ThirdPerson
}
>>>>>>> Stashed changes

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
<<<<<<< Updated upstream
=======
    public Transform ThirdPersonPosition;

    public Vector3 FirstPersonOffset;
    public Vector3 ThirdPersonOffset;

    private Vector3 currentOffset;

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
                DOTween.To(() => currentOffset, x => currentOffset = x,
                    ThirdPersonOffset, 1f);
            }
            else
            {
                DOTween.To(() => currentOffset, x => currentOffset = x,
                    FirstPersonOffset, 1f);
            }

            cameraMode = cameraMode == ECameraMode.FirstPerson ? ECameraMode.ThirdPerson : ECameraMode.FirstPerson;

        }
    }

>>>>>>> Stashed changes

    private void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 newPosition = Target.position;
            transform.position = newPosition;
        }
    }
}
