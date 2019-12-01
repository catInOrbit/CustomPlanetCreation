using SimpleKeplerOrbits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundDetection : MonoBehaviour
{
    //SET PLAYER'S VELOCITY VECTOR TO 0 WHEN PLAYER HIT THE GROUND

    public SphereCollider surfaceCollider;

    public bool isLanded;
    public bool liftOff;

    private bool flag;

    private GameObject ship;

    bool onCollosionEnterBool;
    bool onCollosionExitBool;


    void FixedUpdate()
    {
        StartCoroutine(FreezeVelocityVector());
    }

    private void Update()
    {
        if(onCollosionEnterBool == true)
        {
            StopCoroutine(LiftOffCondition());
            StartCoroutine(LandedCondition());
        }

        if(onCollosionExitBool == true || liftOff == true)
        {
            StopCoroutine(LandedCondition());
            StartCoroutine(LiftOffCondition());
        }
    }

    IEnumerator FreezeVelocityVector()
    {   
        if(isLanded == true)
            ship.gameObject.GetComponent<KeplerOrbitMover>().VelocityHandle.transform.localPosition = new Vector3(0, 0, 0);

        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            ship = collision.gameObject;

            onCollosionEnterBool = true;
            onCollosionExitBool = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            onCollosionEnterBool = false;
            onCollosionExitBool = true;
            liftOff = false;
        }
    }

    //Thread condition for turning off isLanded --> lifting off to prevent deadlock

    IEnumerator LiftOffCondition()
    {

        isLanded = false;
        yield return new WaitForSeconds(0.5f);
    }

    //Thread condition for landed condition --> lifting off to prevent deadlock

    IEnumerator LandedCondition()
    {
        isLanded = true;

        yield return new WaitForSeconds(0.5f);
    }

}
