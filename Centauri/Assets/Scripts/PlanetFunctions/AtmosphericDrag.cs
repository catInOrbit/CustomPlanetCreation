using SimpleKeplerOrbits;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AtmosphericDrag : MonoBehaviour
{

    // REDUCE VELOCITY VECTOR BASES ON HEIGHT FROM PLANET'S SURFACE

    public GameObject player;
    public float dragAmount;


    //Private var
    private bool enableDrag;

    private Transform surfacePoint;
    private Transform atmospherePoint;

    private float playerHeightToGround;
    private float atmosphericHeight;
    private float mainInternalDragForce;

    public float MainInternalDragForce
    {
        get
        {
            return mainInternalDragForce;
        }

        set
        {
            mainInternalDragForce = value;
        }
    }

    public float AtmosphericHeight
    {
        get
        {
            return atmosphericHeight;
        }

        set
        {
            atmosphericHeight = value;
        }
    }

    public float PlayerHeightToGround
    {
        get
        {
            return playerHeightToGround;
        }

        set
        {
            playerHeightToGround = value;
        }
    }

    public bool EnableDrag
    {
        get
        {
            return enableDrag;
        }

        set
        {
            enableDrag = value;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.transform.position, this.GetComponent<SphereCollider>().radius);
    }

    private void Start()
    {
        surfacePoint = gameObject.transform.parent.transform.Find("Surface Point");
        atmospherePoint = gameObject.transform.parent.transform.Find("Atmosphere Point");

        float planetRadius = Vector3.Distance(this.transform.parent.transform.position, surfacePoint.transform.position);

        float atmosphereRadius = Vector3.Distance(this.transform.parent.transform.position, atmospherePoint.transform.position);
        AtmosphericHeight = atmosphereRadius - planetRadius;
    }

    void FixedUpdate ()
    {
        if (EnableDrag == true)
            UpdateVelocityDrag();
    }

    private void UpdateVelocityDrag()
    {

        Vector3 velocityVector = player.transform.Find("Player_VelocityVector").transform.localPosition;
        Vector3 dragVector = -velocityVector;
        float distanceToPlayer = Vector3.Distance(this.transform.parent.transform.position, player.transform.position);

        PlayerHeightToGround = AtmosphericHeight - distanceToPlayer;
        MainInternalDragForce = Mathf.Abs(PlayerHeightToGround) * dragAmount;

        player.transform.Find("Player_VelocityVector").transform.localPosition += new Vector3(dragVector.x * MainInternalDragForce, dragVector.y * MainInternalDragForce);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            EnableDrag = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            EnableDrag = false;
    }

}
