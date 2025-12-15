using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offsetY = 100f;

    private float _minSize = 5f;
    private float _maxSize = 10f;

    private Camera _camera;

    void LateUpdate()
    {
        _camera = GetComponent<Camera>();
        Vector3 targetPosition = _target.position;
        Vector3 finalPosition = _target.position + new Vector3(0f, _offsetY, 0f);

        transform.position = finalPosition;
        Vector3 targetAngle = _target.eulerAngles;
        targetAngle.x = 90;

        transform.eulerAngles = targetAngle;
    }

    public void SizeDown()
    {
        _camera.orthographicSize = Mathf.Min(_camera.orthographicSize + 1f, _maxSize);
    }
    public void SizeUp()
    {
        _camera.orthographicSize = Mathf.Max(_camera.orthographicSize - 1f, _minSize);
    }

}
