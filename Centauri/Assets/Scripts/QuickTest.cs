using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class QuickTest : MonoBehaviour
{

    // The target marker.
    public Transform target;

    // Angular speed in radians per sec.
    public float speed;

    public float rotaionSpeed;


    public Vector3 rotationVector;

    public Vector3 mainAxis;

    public float angle;

    public GameObject assigned;


    void Update()
    {
        Vector3 targetDir = target.position - transform.position;

        // The step size is equal to speed times frame time.
        float step = speed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        Debug.DrawRay(transform.position, newDir, Color.red);

        Debug.DrawRay(target.position, mainAxis * 3, Color.green);



        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);

        transform.RotateAround(target.transform.position, mainAxis, rotaionSpeed * Time.deltaTime);

        Vector3 targetDir2 = target.position - assigned.transform.position;
        float step2 = angle * Time.deltaTime;

        Vector3 newDir2 = Vector3.RotateTowards(assigned.transform.forward, targetDir2, step2, 0.0f);

        assigned.transform.position =  (target.transform.position + mainAxis) * 10;
        assigned.transform.rotation = Quaternion.LookRotation(newDir2);
       
    }

    private void LateUpdate()
    {
        TestInput();
    }

    private void TestInput()
    {
        //Vector3 dir = mainAxis - target.transform.position;
        //if (Input.GetKey(KeyCode.A))
        //{
        //    dir = Quaternion.Euler(angle, 0, 0) * dir;
        //    mainAxis = dir + target.transform.position;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    dir = Quaternion.Euler(-angle, 0, 0) * dir;
        //    mainAxis = dir + target.transform.position;
        //}
        //if (Input.GetKey(KeyCode.W))
        //{
        //    dir = Quaternion.Euler(0, 0, angle) * dir;
        //    mainAxis = dir + target.transform.position;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    dir = Quaternion.Euler(0, 0, -angle) * dir;
        //    mainAxis = dir + target.transform.position;
        //}

        if (Input.GetKey(KeyCode.A))
        {
            mainAxis = Quaternion.Euler(0, angle, 0) * mainAxis;
        }
        if (Input.GetKey(KeyCode.D))
        {
            mainAxis = Quaternion.Euler(0, -angle, 0) * mainAxis;
        }
        if (Input.GetKey(KeyCode.W))
        {
            mainAxis = Quaternion.Euler(0, 0, angle) * mainAxis;
        }
        if (Input.GetKey(KeyCode.S))
        {
            mainAxis = Quaternion.Euler(0, 0, -angle) * mainAxis;
        }

    }
}
