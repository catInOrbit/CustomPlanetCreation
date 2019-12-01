using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OribtTest : MonoBehaviour
{
    public GameObject oribtor;
    public GameObject point;

    public Vector3 axis;
    public float rotationalSpeed;

    public int accuracy;

    public int radius;

    public GameObject tip;

    private Vector3 rotationalPoint;

    void Update()
    {
         oribtor.transform.RotateAround(point.transform.position, axis, rotationalSpeed);
        // int i = 0;
        // if (i <= accuracy)
        // {
        //     i++;
        //     {
        //         float angle = (float)i / (float)accuracy * 2.0f * Mathf.PI;
        //         rotationalPoint = new Vector3(radius * Mathf.Cos(angle) * 5, radius * Mathf.Sin(angle) * 5, 0f);
        //         oribtor.transform.position = rotationalPoint;
        //     }
        // }


        // if (i > accuracy)
        //     i = 0;

    }
}
