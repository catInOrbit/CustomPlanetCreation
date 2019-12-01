using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TestRotationInAction : MonoBehaviour
{
    public Transform rotationalPoint;
    public Transform centerPoint;

    public Vector3 rotationalAxis;

    private TestRotation testRotation;

    public GameObject instantiateObject;

    public float angle;

    private void Start()
    {
        testRotation = new TestRotation(centerPoint.position.x, centerPoint.position.y,
                centerPoint.position.z, rotationalAxis.x, rotationalAxis.y, rotationalAxis.z, angle);
    }

    private void Update()
    {
        double[] pointResult = testRotation.TimesXYZ(rotationalPoint.position.x, rotationalPoint.position.y, rotationalPoint.position.z);

        for (int i = 0; i < 50; i++)
        {
            Vector3 rotatedPoint = new Vector3((float)pointResult[0], (float)pointResult[1], (float)pointResult[2]);
            Instantiate(instantiateObject, rotatedPoint, Quaternion.identity);
            pointResult = testRotation.TimesXYZ(rotatedPoint.x, rotatedPoint.y, rotatedPoint.z);
        }
    }
}