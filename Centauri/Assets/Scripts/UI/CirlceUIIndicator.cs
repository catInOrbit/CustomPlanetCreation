using UnityEngine;
using System.Collections;
using System;

public class CircleUIIndicator : MonoBehaviour
{
    //World position of gameobject
    public Vector3 location;

    public float center;
    public float radius;
    public float innerColor;
    public float radiusWidth;
    

    private void Start() 
    {
        
    }

    private void MoveUIWithLocation()
    {
        this.GetComponent<Material>().SetFloat("_Center", center);
    }

}