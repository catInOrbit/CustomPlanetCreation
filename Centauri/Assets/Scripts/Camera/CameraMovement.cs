using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleKeplerOrbits;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;

    public bool swtichTargetBool;
    public float zoomDistanceAuto;
    public float zoomDistanceFixed;
    public float uiFocusDistance;
    public float uiFocusZoomSpeed;
    public float lockZoomAtSize;
    public float touchZoomSpeed;
    public float dragDampenent3D;
    public float debug3DRotationSpeed;

    public float rotationAngle;
    public float cameraDistance;



    public GameObject debugCurrentCam;

    public bool IsFocusRequested { get; set; }
    public GameObject CurrentCameraObject { get; set; }
    public bool EnableCamMovement { get; set; }
    public bool IsMoving { get; set; }
    public bool UiGainControl { get; set; }
    public bool EnableAutoZoom { get; set; }

    private KeplerOrbitMover keplerOrbitMover;
    private float oldCamSize;
    private float spinAngle;
    private Transform cameraRotationalAxis;
    public float rotationalStep; //RotateTowards

    private InputDetectionUtils inputDetectionUtils = new InputDetectionUtils();

    public Vector3 globalRotationAxis;

    private Vector3 rotation;
    void Start()
    {

        EnableCamMovement = true;
        CurrentCameraObject = GameObject.FindGameObjectWithTag("SurfacePoint");

        globalRotationAxis = new Vector3(CurrentCameraObject.transform.position.x, CurrentCameraObject.transform.position.y * 2, CurrentCameraObject.transform.position.z);

    }
    void Update()
    {
        inputDetectionUtils.TouchCountDetection();
        // SetCameraRotationalAxis(CurrentCameraObject.transform);
       Camera3DMovement();
    }

    void LateUpdate()
    {
        ZoomEffect(EnableAutoZoom, UiGainControl);

        if (IsFocusRequested && !IsMoving)
            FocusToNewTarget(CurrentCameraObject.transform, 0.2f);
    }

    Vector3 GetCenterTargetPosition(Transform target)
    {
        return new Vector3(target.transform.position.x - 10, target.transform.position.y - 10, target.transform.position.z - 10);
    }

    void ZoomEffect(bool isAutoZoom, bool uiGainControl)
    {
        if (!isAutoZoom && uiGainControl == false)
        {
            PinchZoom();
        }

        if (uiGainControl == true)
        {
        }
    }

    private void Camera3DMovement()
    {
        inputDetectionUtils.ProcessTouchStateTwoPoints();

        rotation += new Vector3(inputDetectionUtils.MoveDirection.y, inputDetectionUtils.MoveDirection.x, 0) * Time.deltaTime * dragDampenent3D;

        Debug.Log( inputDetectionUtils.MoveDirection);

        #if UNITY_EDITOR
            rotation += new Vector3(-Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"), 0) * Time.deltaTime * debug3DRotationSpeed;
        #endif

        transform.position = CurrentCameraObject.transform.position + Quaternion.Euler(rotation) * new Vector3(0,0, cameraDistance);
        transform.rotation = Quaternion.LookRotation(CurrentCameraObject.transform.position - transform.position);
    }

    void PinchZoom()
    {
        if (Input.touchCount == 2)
        {
            IsMoving = true;
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            if(Camera.main.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.    
                this.GetComponent<Camera>().orthographicSize += deltaMagnitudeDiff * touchZoomSpeed;
                // Make sure the orthographic size never drops below zero.
                this.GetComponent<Camera>().orthographicSize = Mathf.Max(this.GetComponent<Camera>().orthographicSize, 0.1f);
            }

            else
            {
                //this.GetComponent<Camera>().transform.position = Vector3.MoveTowards(transform.position, CurrentCameraObject.transform.position, -deltaMagnitudeDiff * touchZoomSpeed);
                this.GetComponent<Camera>().fieldOfView += deltaMagnitudeDiff * touchZoomSpeed;
            }
        }

        else
            IsMoving = false;
    }

    private Vector3 SetLerpPosition(Transform target, float lerpTime)
    {
        return Vector3.Lerp(this.transform.position, target.transform.position - new Vector3(0, 0, 10), lerpTime);
    }

    public void FocusToNewTarget(Transform target, float lerpTime)
    {
        IsFocusRequested = true;
        // Debug.Log("FocusToNewTarget is running");

        StartCoroutine(LookAtTarget(target, 0.6f));

        //LookAtTarget(target);

        Vector3 currentCamPos = this.GetComponent<Camera>().transform.position;
        CurrentCameraObject = target.gameObject;

        if (Vector3.Distance(this.transform.position, GetCenterTargetPosition(target)) > 0.5f)
        {
            this.GetComponent<Camera>().transform.position = SetLerpPosition(target, 0.1f);
        }
      
    }

    private IEnumerator LookAtTarget(Transform target, float rotationalSpeed)
    {
    

        Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - this.transform.position);

        this.transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationalSpeed * Time.deltaTime);
        yield return null;

    }

    private void LockOnToTarget(Transform target)
    {
        this.transform.position = GetCenterTargetPosition(target);
    }

    private void SetCameraRotationalAxis(Transform target)
    {
        cameraRotationalAxis = target.transform;
    }

    private string TiltDirection(Vector2 touch0, Vector2 touch1)
    {
        if(touch0.x > touch1.x)
            return "Right";
        else
            return "Left";
    }

}
