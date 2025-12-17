using UnityEngine;

public enum ECameraMode
{
    FirstPerson,
    ThirdPerson,
    TopView,
}
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public ECameraMode CameraMode = ECameraMode.FirstPerson;

  

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    void Update()
    {
        
    }
}
