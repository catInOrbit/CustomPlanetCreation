using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTest : MonoBehaviour
{
    void Update()
    {
        Debug.Log("Angle to right vector: " + GetAngleToVectorRight(this.transform.position));
        Debug.Log("Angle to up vector: " + GetAngleToVectorUp(this.transform.position));
    }

    private float GetAngleToCompareVector(Vector3 targetDirectionVector, Vector3 compareVector)
    {
        return Mathf.Acos(Vector3.Dot(targetDirectionVector, compareVector) / (targetDirectionVector.magnitude * compareVector.magnitude)) * Mathf.Rad2Deg;
    }

    private float GetAngleToVectorRight(Vector3 targetDirectionVector)
    {
        return Mathf.Acos(Vector3.Dot(targetDirectionVector, Vector3.right) / (targetDirectionVector.magnitude * Vector3.right.magnitude)) * Mathf.Rad2Deg;
    }

    private float GetAngleToVectorUp(Vector3 targetDirectionVector)
    {
        return Mathf.Acos(Vector3.Dot(targetDirectionVector, Vector3.up) / (targetDirectionVector.magnitude * Vector3.up.magnitude)) * Mathf.Rad2Deg;
    }


}
