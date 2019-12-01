using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalPull : MonoBehaviour
{
    public GameObject[] gameObjects;

    private float forceAmount;
    private float gravityUpscaleFactor;

    const float gravityConstant = 6.7f;
    private Vector3 gravityVector;
    private Vector3 difference;

    private float gravity;
    private float radiusBetweenBody;

    public float ForceAmount
    {
        get
        {
            return forceAmount;
        }

        set
        {
            forceAmount = value;
        }
    }

    public float GravityUpscaleFactor
    {
        get
        {
            return gravityUpscaleFactor;
        }

        set
        {
            gravityUpscaleFactor = value;
        }
    }

    public static float GravityConstant
    {
        get
        {
            return gravityConstant;
        }
    }

    public Vector3 GravityVector
    {
        get
        {
            return gravityVector;
        }

        set
        {
            gravityVector = value;
        }
    }

    public Vector3 Difference
    {
        get
        {
            return difference;
        }

        set
        {
            difference = value;
        }
    }

    public float Gravity
    {
        get
        {
            return gravity;
        }

        set
        {
            gravity = value;
        }
    }

    public float RadiusBetweenBody
    {
        get
        {
            return radiusBetweenBody;
        }

        set
        {
            radiusBetweenBody = value;
        }
    }

    void Update()
    {
        GravitationalEffect();
    }


    private void GravitationalEffect()
    {
        foreach (GameObject body in gameObjects)
        {
            //Radius bewteen each body
            Difference = this.transform.position - body.transform.position;

            //Distance = magnitude
            RadiusBetweenBody = Difference.magnitude;

          

            //Direction is a normalized vector
            Vector3 gravityDirection = Difference.normalized;


            if (Input.GetKey(KeyCode.A))
            {
                //gravityDirection += new Vector3(1, 0, 0);
                body.transform.GetComponent<Rigidbody>().AddForce(new Vector3(2, 0, 0), ForceMode.Acceleration);
            }

            Gravity = (GravityConstant * (this.transform.localScale.x * body.transform.localScale.x * GravityUpscaleFactor)) / (RadiusBetweenBody * RadiusBetweenBody);

            GravityVector = (gravityDirection * Gravity);

            
            body.transform.GetComponent<Rigidbody>().AddForce(GravityVector, ForceMode.Acceleration);
        }
    }

}
