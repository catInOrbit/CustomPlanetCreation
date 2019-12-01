using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 /// <summary>
 ///Class that detects and stores all touch related activities
    /// <para>!! Required function in Update() thread:</para> 
    /// <example>
    /// <code>
    /// public string ProcessTouchStateOnePoint()
    /// </code>
    /// </example>
/// </summary>
public class InputDetectionUtils
{
    private float forceStrength;
    public Vector3 MoveDirection { get; set; }
    public Vector3 TouchEndPos { get; set; }
    public Vector3 TouchStartPos { get; set; }

    public static string Begin { get { return "daw";}}
    public static string Moving { get { return "Moving";}}
    public static string Holding { get { return "Holding";}}
    public static string Ended { get { return "Ended";}}
    public static string Canceled { get { return "Canceled";}}

    public float ForceStrength
    {
        get
        {
            return forceStrength;
        }

        set
        {
            forceStrength = value;
        }
    }

    public int TouchCount { get; set; }

    public bool TouchStopped { get; set; }

/// <summary>Current support for 2 touch states.
///Else return invlid / no touch state -1</summary>
///
    public void TouchCountDetection()
    {
        //Current support for 2 touch state
        //Debug.Log("TouchcountDetection is running");
        if(Input.touchCount == 1)
            TouchCount = 1;
        if(Input.touchCount == 2)
        {
            TouchCount = 2;
            Debug.Log("TouchcountDetection touchcount 2");
        }
        else
            TouchCount = -1;
    }

    public string ProcessTouchStateOnePoint()
    {
        string returnState = "";
        if (Input.touchCount == 1)
        {
            TouchStopped = false;

            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TouchStartPos = new Vector3(touch.position.x, touch.position.y, 0);
                    returnState = "Begin";
                    break;
                case TouchPhase.Moved:
                    TouchEndPos = new Vector3(touch.position.x, touch.position.y, 0);
                    ForceStrength = Vector2.Distance(TouchStartPos, TouchEndPos);
                    MoveDirection = TouchEndPos - TouchStartPos;
                    returnState = "Moving";

                    break;
                case TouchPhase.Stationary:
                    TouchEndPos = new Vector3(touch.position.x, touch.position.y, 0);
                    MoveDirection = TouchEndPos - TouchStartPos;
                    returnState = "Holding";
                    break;
                case TouchPhase.Ended:
                    returnState = "Ended";
                    TouchStopped = true;

                    break;
                case TouchPhase.Canceled:
                    returnState = "Canceled";
                    TouchStopped = true;

                    break;
                default:
                    break;
            }
        }

        return returnState;
    }

    public string ProcessTouchStateTwoPoints()
    {
        string returnState = "";
        if (Input.touchCount == 2)
        {
            TouchStopped = false;

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TouchStartPos = new Vector3(touchOne.position.x, touchOne.position.y, 0);
                    returnState = "Begin";
                    break;
                case TouchPhase.Moved:
                    TouchEndPos = new Vector3(touchOne.position.x, touchOne.position.y, 0);
                    ForceStrength = Vector2.Distance(TouchStartPos, TouchEndPos);
                    MoveDirection = TouchEndPos - TouchStartPos;
                    returnState = "Moving";
                    break;
                case TouchPhase.Stationary:
                    TouchEndPos = new Vector3(touchOne.position.x, touchOne.position.y, 0);
                    ForceStrength = Vector2.Distance(TouchStartPos, TouchEndPos);
                    MoveDirection = TouchEndPos - TouchStartPos;
                    returnState = "Holding";

                    break;
                case TouchPhase.Ended:
                    returnState = "Ended";
                     MoveDirection = new Vector3(0,0,0);
                    TouchStopped = true;

                    break;
                case TouchPhase.Canceled:
                    returnState = "Canceled";
                    TouchStopped = true;

                    break;
                default:
                    break;
            }
        }

        return returnState;
    }

    
      /// <summary>
    /// Detect which side of the screen touch is recordred
    /// <para>Return string stating Left or Right</para> 
    /// </summary>
    public string ScreenSideDetection(float screenWidth)
    {
        if(TouchStartPos.x <= screenWidth / 2)
        {
            //Debug.Log(TouchStartPos.x + "----" + screenWidth / 2 + "--- LEFT");
            return "Left";
        }
        else
        {
            //Debug.Log(TouchStartPos.x + "----" + screenWidth / 2 + "--- RIGHT");
            return "Right";
        }
    }
}
