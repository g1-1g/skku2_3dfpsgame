using System;
using UnityEngine;

public class TryCatchFinallyTest : MonoBehaviour
{

    public int Age; 
    void Start ()
    {
       if (Age < 0)
        {
            Debug.LogError("Age cannot be negative.");
            throw new Exception ("Age cannot be negative.");
        }


            try
        {
            int[] numbers = new int[32];
            numbers[75] = 1;
            
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.LogError("Caught a IndexOutOfRangeException: " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Caught a general exception: " + ex.Message);
        }
        finally //옵션
        {
            Debug.Log("In finally block - this always executes.");
        }
    }
}
