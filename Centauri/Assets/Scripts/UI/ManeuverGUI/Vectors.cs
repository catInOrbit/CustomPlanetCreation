using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vectors : MonoBehaviour
{
    private GameObject velocityVector;
    private GameObject headingVector;

    private void Start() 
    {
        velocityVector = GameObject.Find("/Player/VelocityArrow");
        headingVector = GameObject.Find("/Player/HeadingVector");
    }

    private void Update() 
    {
        DrawVelocity();
        DrawHeading();
    }

     /// <summary>
     ///Set position for drawing line using LineRenderer
    /// </summary>
    void SetDrawPosition(LineRenderer gameojComponent, Vector3 position0, Vector3 position1)
    {
        gameojComponent.SetPosition(0, position1);
        gameojComponent.SetPosition(1, position0);
    }


    void DrawVelocity()
    {
        SetDrawPosition(velocityVector.GetComponent<LineRenderer>(), velocityVector.transform.position, velocityVector.transform.parent.transform.position);
    }

    void DrawHeading()
    {
        SetDrawPosition(headingVector.GetComponent<LineRenderer>(), headingVector.transform.position, headingVector.transform.parent.transform.position);
    }

}
