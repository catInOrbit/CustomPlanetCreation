using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class PlayerHeading : MonoBehaviour 
{

    private void Update() 
    {
        DrawHeadingVector();
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(this.transform.parent.transform.position, this.transform.position);
    }

    private void DrawHeadingVector()
    {
        this.GetComponent<LineRenderer>().SetPosition(0, this.transform.parent.transform.position);
        this.GetComponent<LineRenderer>().SetPosition(1, this.transform.position);
    }
}


