using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbitalManeuverV2 : MonoBehaviour
{
    // public Text debuggerConsole; //TODO:  Delete this
    private GameObject player;
    [Header("Speed of rotation")]
    public float rotationalSpeed;
    [Header("Bounding box size")]
    public float boundingBoxFloatConst;

    public GameObject boundingBoxGameobject;

    public float rotationalSpeedDampenent;

    public float forwardForceDampenent;

    private bool isOutOfBound = false;
    private InputDetectionUtils inputDetectionUtils = new InputDetectionUtils();
    private ScreenUtils screenUtils = new ScreenUtils(Screen.width, Screen.height);

    private RotationMatrixHelper rotationMatrixHelper = new RotationMatrixHelper();


    //----------------Set of fields designated for MoveVelVectorTowardsDesinatedPos()--------------
    private double[] points;
    private Vector3 rotatedPoint;

    //----------------Set of fields designated for MoveVelVectorTowardsDesinatedPos()--------------


    [SerializeField] //For showing in inspector for debug
    private Vector3 axisOfRotation;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // SetAxisOfRotation();
        SetRotationalSpeed();
        SetRotationalMatrix();
        if (inputDetectionUtils.ProcessTouchStateOnePoint() == "Moving" ||
            inputDetectionUtils.ProcessTouchStateOnePoint() == "Hodling")
        {
            if (inputDetectionUtils.ScreenSideDetection(Screen.width) == "Left")
                EngineBurn();
            if (inputDetectionUtils.ScreenSideDetection(Screen.width) == "Right")
                MoveVelVectorTowardsDesinatedPos(axisOfRotation);
        }

        // UpdateDebugConsole();
        DebugKeyboardInput();
    }


    private void SetRotationalSpeed()
    {
        rotationalSpeed = inputDetectionUtils.MoveDirection.magnitude;
    }

    /// <summary>
    /// Main method to handle rotation.
    ///Rotate around redefined axis of rotation at speed:
    /// <param name="rotationalSpeed">The speed of rotation</param>
    /// <para>Axis is set in code:</para> 
    /// <example>
    /// <code>
    ///   private void SetAxisOfRotation()
    ///   {
    ///     axisOfRotation = inputDetectionUtils.MoveDirection.normalized;
    ///   }
    /// </code>
    /// </example>
    /// </summary>
    private void MoveVelVectorTowardsDesinatedPos(Vector3 axisOfRotation)
    {
        GameObject tempHeadingVector = player.transform.Find("HeadingVector").gameObject;
        float tempForceStrength = inputDetectionUtils.ForceStrength;
        points = rotationMatrixHelper.TimesXYZ(tempHeadingVector.transform.position.x, 
                tempHeadingVector.transform.position.y,tempHeadingVector.transform.position.z);
        if (inputDetectionUtils.TouchStopped != true)
        {
            rotatedPoint = new Vector3((float)points[0], (float)points[1], (float)points[2]);
            player.transform.Find("HeadingVector").transform.position = rotatedPoint;
            points = rotationMatrixHelper.TimesXYZ(rotatedPoint.x, rotatedPoint.y, rotatedPoint.z);
        }

        // player.transform.Find("HeadingVector").transform.position = rotatePointAroundAxis(player.transform.position, 360, new Vector3(1,0.5f,0));


    }
    // private Vector3 rotatePointAroundAxis(Vector3 point, float angle, Vector3 axis)
    // {
    //     Quaternion q = Quaternion.AngleAxis(angle, axis);
    //     return q * point; //Note: q must be first (point * q wouldn't compile)
    // }

    /// <summary>
    ///Set properties defined in class:.
    /// <para>RotationMatrixHelper.cs</para> 
    /// </summary>
    private void SetRotationalMatrix()
    {
        Vector3 tempFinalVec = CalculateFinalRotationVector(inputDetectionUtils.MoveDirection);
        rotationMatrixHelper.AcceptInput(player.transform.position.x, player.transform.position.y, player.transform.position.z, tempFinalVec.x,
            tempFinalVec.y, tempFinalVec.z, rotationalSpeed * rotationalSpeedDampenent);
    }



    /// <summary>
    /// Setting the axis upon which player's heading vector will rotate around.
    /// <para>Currently = </para> 
    /// <example>
    /// <code>
    /// inputDetectionUtils.MoveDirection.normalized;
    /// </code>
    /// </example>
    /// </summary>
    // private void SetAxisOfRotation()
    // {
    //     //axisOfRotation =;
    //     axisOfRotation = CalculateFinalRotationVector(inputDetectionUtils.MoveDirection.normalized);
    // }

    // private void UpdateDebugConsole() //TODO: Remove this debug function when no longer needed
    // {
    //     string rotationalVec = "Rotational axis: \n" + axisOfRotation.ToString();
    //     string screen14 = "\n Screen 14: \n" + screenUtils.GetScreenConstraint14(Screen.width, Screen.height).ToString();
    //     debuggerConsole.gameObject.GetComponent<Text>().text = axisOfRotation + screen14;
    // }

    private void DebugKeyboardInput()
    {
        //TODO: Remove this debug function when no longer needed
        if (Input.GetKeyDown(KeyCode.W))
        {
            axisOfRotation += new Vector3(3f, 1f, 0);
        }
    }

    /// <summary>
    /// Calculate the axis of rotating (perpendicular to the swipe direction)
    /// <example>
    /// <code>
    ///  x' = x cos θ − y sin θ
    ///  y' = x sin θ + +y cos θ
    /// </code>
    /// </example>
    /// </summary>
    private Vector3 CalculateFinalRotationVector(Vector3 initialVector)
    {
        //Assume  θ = 90
        float newX = /* initialVector.x * Mathf.Cos(90);*/ 0 - initialVector.y * 1;
        float newY = initialVector.x + 0;

        return new Vector2(newX, newY);
    }

    private void EngineBurn() //TODO: Retrograde burn
    {
        Debug.Log("Engine burn");
        Vector3 headingVector = player.transform.Find("HeadingVector").transform.position;
        Vector3 burnVector = headingVector - player.transform.position;
        float force = inputDetectionUtils.ForceStrength;

        Vector3 finalBurnVector = new Vector3(burnVector.normalized.x * forwardForceDampenent * force,
        burnVector.normalized.y * forwardForceDampenent * force, burnVector.normalized.z * forwardForceDampenent * force);

        if(inputDetectionUtils.MoveDirection.y > 0)
            player.transform.Find("Player_VelocityVector").transform.position += finalBurnVector;

        else
            player.transform.Find("Player_VelocityVector").transform.position -= finalBurnVector;
    }

}
