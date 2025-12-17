using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerTopViewMove : MonoBehaviour
{
    private ECameraMode _cameraMode;
    private Camera _camera;

    private LayerMask _mask;

    private NavMeshAgent _agent;

    private PlayerStats _stats;
 
    void Start()
    {
        CameraManager.Instance.OnCameraModeChanged += CameraModeChanged;
        _camera = Camera.main;
        _cameraMode = CameraManager.Instance.CameraMode;
        _stats = GetComponent<PlayerStats>();
        
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _stats.MoveSpeed.Value;

        _mask = (1 << LayerMask.NameToLayer("Ground"));

    }
    private void CameraModeChanged(ECameraMode cameraMode)
    {
        _cameraMode = cameraMode;
    }


    void Update()
    {
        if (_cameraMode != ECameraMode.TopView) return;
        if (Input.GetMouseButton(1))
        {
            _agent.ResetPath();
            Vector3 mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.nearClipPlane));
            Vector3 dir = (mousePos - _camera.transform.position).normalized;

            Debug.DrawRay(_camera.transform.position, dir * 100.0f, Color.red, 1.0f);
            RaycastHit hit;
            Ray ray = new Ray(_camera.transform.position, dir);
            if (Physics.Raycast(ray, out hit, 100.0f, _mask))
            {
                Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");
                _agent.SetDestination(hit.point);
            }
        }
    }

    private void OnDestroy()
    {
        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.OnCameraModeChanged -= CameraModeChanged;
        }
    }
}
