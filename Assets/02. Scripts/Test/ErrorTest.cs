using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ErrorTest : MonoBehaviour
{
    void Start()
    {
        //MissingComponentException
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        //Debug.Log(rigidbody.linearVelocity);

        //nullReferenceException
        Rigidbody2D rigidbody2 = null;
       // Debug.Log(rigidbody2.linearVelocity);

    }
}
