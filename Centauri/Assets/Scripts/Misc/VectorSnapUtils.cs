using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorSnapUtils : MonoBehaviour
{
    //public Vector3 direction, up;

    //void Update()
    //{
    //    direction = this.transform.position;
    //    Debug.Log(CheckHorizontalSnap(direction) + "Hor");
    //    Debug.Log(CheckVerticalSnap(direction) + "Ver");
    //    Debug.Log(CheckSkewedSnap(direction) + "Skewded");
    //}

    private float GetAngleToCompareVector(Vector3 targetDirectionVector, Vector3 compareVector)
    {
        return Mathf.Acos(Vector3.Dot(targetDirectionVector, compareVector) / (targetDirectionVector.magnitude * compareVector.magnitude));
    }

    private float GetAngleToVectorRight(Vector3 targetDirectionVector)
    {
        return Mathf.Acos(Vector3.Dot(targetDirectionVector, Vector3.right) / (targetDirectionVector.magnitude * Vector3.right.magnitude));
    }

    private float GetAngleToVectorUp(Vector3 targetDirectionVector)
    {
        return Mathf.Acos(Vector3.Dot(targetDirectionVector, Vector3.up) / (targetDirectionVector.magnitude * Vector3.up.magnitude));
    }

    public bool CheckHorizontalSnap(Vector3 direction)
    {
        if ((GetAngleToVectorRight(direction) >= 0 && (GetAngleToVectorUp(direction) >= 80 && GetAngleToVectorUp(direction) <= 110)))
            return true;

        return false;
    }

    public bool CheckVerticalSnap(Vector3 direction)
    {
        if ((GetAngleToVectorUp(direction) >= 0) && (GetAngleToVectorRight(direction) >= 80 && GetAngleToVectorRight(direction) <= 110))
            return true;
        return false;
    }

    public bool CheckSkewedSnap(Vector3 direction)
    {
        //if(GetAngleToVectorUp(direction) >= 20 )
        return false;
    }
}
