using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(SphereCollider))]
public class PlayerUIDetection : MonoBehaviour
{

    public float colliderScaleAmount;
    private UITouchManager uiTouchManager;
    private OrbitalManeuver orbitalManeuver;
    private CameraMovement cameraMovement;
    private const int _playerSelectionLayer = 11;
    private GameObject maneuverBubble;

    private InputDetectionUtils inputDetectionUtils = new InputDetectionUtils();

    private void Start()
    {
        orbitalManeuver = GameObject.FindObjectOfType<OrbitalManeuver>();
        uiTouchManager = GameObject.FindObjectOfType<UITouchManager>();
        cameraMovement = GameObject.FindObjectOfType<CameraMovement>();
        maneuverBubble = GameObject.Find("Player").gameObject.transform.Find("ManeuverFx").gameObject;
    }

    void Update()
    {
        ScaleColliderWithScreen(6.2f);
        if(inputDetectionUtils.ProcessTouchStateOnePoint() == "Begin")
            RayCastToCollider(_playerSelectionLayer);
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
            RayCastToCollider(_playerSelectionLayer);
#endif
    }

    // void OnTouchDown()
    // {
    //     SendActivateSignal();
    // }

    // void OnTouchUp()
    // {
    //     SetCameraFocusExitConditions();
    // }

    void SendActivateSignal()
    {
        if (!cameraMovement.IsMoving)
        {
            uiTouchManager.DisplayUIOptions = true;
            uiTouchManager.CancelBool = false;
            //orbitalManeuver.enabled = false;

            uiTouchManager.ActivationMessageQueue.Enqueue(true);
            cameraMovement.IsFocusRequested = true;
            cameraMovement.CurrentCameraObject = this.gameObject;
            EnableManeuverBubbleFx();
        }
    }

    private void ScaleColliderWithScreen(float scaleFactor)
    {
        float currentCameraOrthoSize = Camera.main.orthographicSize;
        this.gameObject.GetComponent<SphereCollider>().radius = currentCameraOrthoSize * colliderScaleAmount;
    }


    private void RayCastToCollider(int layerIntIndex)
    {
        int layerMaskBitShift = 1 << layerIntIndex; //Collider only with layerIntIndex
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(inputDetectionUtils.TouchStartPos), out hit, Mathf.Infinity, layerMaskBitShift))
            SendActivateSignal();
        else
           SetFocusExitConditions();
#if UNITY_EDITOR
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMaskBitShift))
            SendActivateSignal();
#endif
    }

    private void SetFocusExitConditions()
    {
        cameraMovement.IsFocusRequested = false;
        uiTouchManager.CancelBool = true;
        DeactivateManeuverBubbleFx();
    }

    private void EnableManeuverBubbleFx()
    {
        maneuverBubble.gameObject.SetActive(true);
    }

    private void DeactivateManeuverBubbleFx()
    {
        maneuverBubble.gameObject.SetActive(false);
    }

}
