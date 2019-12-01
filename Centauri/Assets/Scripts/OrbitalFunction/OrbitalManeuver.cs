using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleKeplerOrbits;
using System.Threading;

public class OrbitalManeuver : MonoBehaviour
{
    /* THIS CLASS IS USED IN KEPLERORBITMOVER */


    //Publics
    public float debugLockEngine;
    public float forceDampenent;
    public float mobileFloatDampenent;

    public GameObject velocityVector;
    private Vector3 mouseMoveDirection;
    private OnGroundDetection onGroundDetection;
    private ShipData shipData;

    Vector3 mouseStartPos;
    Vector3 mouseEndpos;

    Vector2 touchStartPos;
    Vector2 touchEndPos;

    //Props

    public Vector3 TempTouchDirection { get; set; }
    public Vector3 TouchDirection { get; set; }

    public float VectorMagnitudeCap { get; set; }
    public float ForceStrength { get; set; }

    public Vector3 Prograde
    {
        get
        {
            return prograde;
        }

        set
        {
            prograde = value;
        }
    }
    public Vector3 RetroGrade
    {
        get
        {
            return retroGrade;
        }

        set
        {
            retroGrade = value;
        }
    }
    public Vector3 RadialIn
    {
        get
        {
            return radialIn;
        }

        set
        {
            radialIn = value;
        }
    }
    public Vector3 RadialOut
    {
        get
        {
            return radialOut;
        }

        set
        {
            radialOut = value;
        }
    }
    public Vector3 NormalUp
    {
        get
        {
            return normalUp;
        }

        set
        {
            normalUp = value;
        }
    }
    public Vector3 NormalDown
    {
        get
        {
            return normalDown;
        }

        set
        {
            normalDown = value;
        }
    }

    private Vector3 prograde, retroGrade, radialIn,
        radialOut, normalUp, normalDown;

    private Touch getTouch;
    private VectorSnapUtils vectorSnapUtils = new VectorSnapUtils();

    private void Awake()
    {
        VectorMagnitudeCap = GetScreenConstraint14(Screen.width, Screen.height);
        Debug.Log(VectorMagnitudeCap);
    }

    private void Start()
    {
        onGroundDetection = GameObject.FindObjectOfType<OnGroundDetection>();
        shipData = this.GetComponent<ShipData>();
    }

    void Update()
    {
        if ((onGroundDetection.isLanded == false && onGroundDetection.liftOff == false) || onGroundDetection.liftOff == true)
        {
#if UNITY_EDITOR
            MouseInput();
#endif
            TouchInput();

        }

        if(onGroundDetection.liftOff == true)   
            PointVelocityVectorToTarget(velocityVector);

        //NeutralizeVelocityZAxis(velocityVector);
    }


    void TouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            getTouch = touch;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = new Vector3(touch.position.x, touch.position.y, 0);
                    break;
                case TouchPhase.Moved:
                    touchEndPos = new Vector3(touch.position.x, touch.position.y, 0);
                    ForceStrength = Vector2.Distance(touchStartPos, touchEndPos);
                    if (ForceStrength <= VectorMagnitudeCap)
                        TouchDirection = touchEndPos - touchStartPos;
                    else
                        ForceStrength = VectorMagnitudeCap;
                    break;
                case TouchPhase.Stationary:
                    touchEndPos = new Vector3(touch.position.x, touch.position.y, 0);
                    ForceStrength = Vector2.Distance(touchStartPos, touchEndPos);

                    if (ForceStrength <= VectorMagnitudeCap)
                        TouchDirection = touchEndPos - touchStartPos;
                    else
                        ForceStrength = VectorMagnitudeCap;

                    break;
                case TouchPhase.Ended:
                    TouchDirection = new Vector3(0, 0, 0);
                    ForceStrength = 0;
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
        }

        else
            getTouch.position = new Vector3(0, 0, 0);
    }

    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
            mouseStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            mouseEndpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseMoveDirection = mouseEndpos - mouseStartPos;
            //TempTouchDirection = mouseMoveDirection;

            ForceStrength = Vector3.Distance(mouseEndpos, mouseStartPos);

            if (ForceStrength <= VectorMagnitudeCap)
                mouseMoveDirection = mouseEndpos - mouseStartPos;

            else
                //mouseMoveDirection = TempTouchDirection;
                ForceStrength = VectorMagnitudeCap;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseMoveDirection = new Vector3(0, 0, 0);
            ForceStrength = 0;
        }
    }

    public void PointVelocityVectorToTarget(GameObject velocityVector)
    {
        //velocityVector.transform.position += new Vector3(
        //        TouchDirection.x * mobileFloatDampenent,
        //        TouchDirection.y * mobileFloatDampenent,
        //        0
        //    );


        if (getTouch.position.x < GetScreenHalfpoint(Screen.width, Screen.height).x)
        {
            //if(!RadialManeuver(TouchDirection) && !)
        }
    }

    private bool RadialManeuver(GameObject velocityVector)
    {
        if (vectorSnapUtils.CheckHorizontalSnap(TouchDirection))
        {
            Debug.Log("Non Pro/Retro Horizontal");
            if (TouchDirection.x > 0)
                velocityVector.transform.position += SetRadialOut(velocityVector, out radialOut);
            if (TouchDirection.x < 0)
                velocityVector.transform.position += SetRadialIn(velocityVector, out radialIn);

            return true;
        }

        return false;
    }

    private bool NormalManeuver()
    {

        Debug.Log("Non Pro/Retro Skewed");

        if (TouchDirection.x > 0)
            velocityVector.transform.position += SetNormalUp(velocityVector, out normalUp);
        if (TouchDirection.x < 0)
            velocityVector.transform.position += SetNormalDown(velocityVector, out normalDown);

        return false;
    }



    private bool ProgradeManeuver(GameObject velocityVector)
    {
        if (vectorSnapUtils.CheckVerticalSnap(TouchDirection))
        {
            Debug.Log("Non Pro/Retro Vertical");

            if (TouchDirection.y > 0)
                velocityVector.transform.position += SetPrograde(velocityVector, out prograde);
            if (TouchDirection.y < 0)
                velocityVector.transform.position += SetRetrograde(velocityVector, out retroGrade);

            return true;
        }

        return false;
    }

    //One fourth of the screen
    public float GetScreenConstraint14(float width, float height)
    {
        return Mathf.Sqrt(Mathf.Pow(width, 2) + Mathf.Pow(height, 2)) / 4;
    }

    public Vector2 GetScreenHalfpoint(float width, float height)
    {
        return new Vector2(width / 2, height / 2);
    }

    private void NeutralizeVelocityZAxis(GameObject velocityVector)
    {
        if (velocityVector.transform.position.z != 0)
            velocityVector.transform.position = new Vector3(velocityVector.transform.position.x, velocityVector.transform.position.y, 0);
    }
    public void SetForceDampenent(float dampenent)
    {
        mobileFloatDampenent = dampenent;
    }

    public Vector3 GetVelocityVectorPosition()  
    {
        return velocityVector.transform.position;
    }

    public Vector3 SetPrograde(GameObject velocityVector, out Vector3 prograde)
    {
        prograde = new Vector3(ForceStrength * mobileFloatDampenent,
             + ForceStrength * mobileFloatDampenent, 0);
        return prograde;
    }
    public Vector3 SetRetrograde(GameObject velocityVector, out Vector3 retrograde)
    {
        retrograde = new Vector3( - ForceStrength * mobileFloatDampenent,
             - ForceStrength * mobileFloatDampenent, 0);
        return retrograde;
    }
    public Vector3 SetRadialIn(GameObject velocityVector, out Vector3 radialIn)
    {
        radialIn = new Vector3( -ForceStrength * mobileFloatDampenent,
            0, 0);
        return radialIn;
    }
    public Vector3 SetRadialOut(GameObject velocityVector, out Vector3 radialOut)
    {
        radialOut = new Vector3(ForceStrength * mobileFloatDampenent,
            0, 0);
        return radialOut;
    }
    public Vector3 SetNormalUp(GameObject velocityVector, out Vector3 normalUp)
    {
        normalUp = new Vector3(0,
          0, ForceStrength * mobileFloatDampenent);
        return normalUp;
    }
    public Vector3 SetNormalDown(GameObject velocityVector, out Vector3 normalDown)
    {
        normalDown = new Vector3(0,
           0, -ForceStrength * mobileFloatDampenent);
        return normalDown;
    }

}
