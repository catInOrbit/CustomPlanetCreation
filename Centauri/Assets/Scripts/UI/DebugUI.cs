using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugUI : MonoBehaviour
{
    public InputField velocityDampent;
    public Button liftOff;
    //public Button cameraAutoZoom;
    //public Text debug;

    private OrbitalManeuver orbitalManeuver;
    private OnGroundDetection onGroundDetection;
    private CameraMovement cameraMovement;
    private SpeedData speedData;

	// Use this for initialization
	void Start ()
    {
        orbitalManeuver = GameObject.FindObjectOfType<OrbitalManeuver>();
        onGroundDetection = GameObject.FindObjectOfType<OnGroundDetection>();
        cameraMovement = GameObject.FindObjectOfType<CameraMovement>();
        speedData = GameObject.FindObjectOfType<SpeedData>();

        liftOff.onClick.AddListener(EnableLiftOff);
        //cameraAutoZoom.onClick.AddListener(AutoZoom);
    }

    private void Update()
    {
        //DisplayDebugInfo(orbitalManeuver.TouchDirection, orbitalManeuver.ForceStrength, orbitalManeuver.TempTouchDirection, speedData.GetEngineThrustPercentage(), orbitalManeuver.VectorMagnitudeCap);
    }

    public void UpdateVelocityDampenent()
    {
        orbitalManeuver.mobileFloatDampenent = float.Parse(velocityDampent.text);
    }

    public void EnableLiftOff()
    {
        if (onGroundDetection.liftOff == true)
            onGroundDetection.liftOff = false;
        if(onGroundDetection.liftOff == false)
            onGroundDetection.liftOff = true;
    }

    public void AutoZoom()
    {
        if (cameraMovement.EnableAutoZoom == false)
            cameraMovement.EnableAutoZoom = true;

        if (cameraMovement.EnableAutoZoom == true)
            cameraMovement.EnableAutoZoom = false;
    }

    //public void DisplayDebugInfo(Vector3 touchDir, float forceStrength, Vector3 lockVec, string thrustPercentage, float vecCap)
    //{
    //    string displayString = string.Format("Touch direction vector: {0} \r\n Force strength: {1} \r\n Lock vector: {2} \r\n Force " +
    //        "percentatge: {3} \r\n Veccap {4}", touchDir.ToString(), forceStrength, lockVec.ToString(), thrustPercentage, vecCap);
    //    debug.text = displayString;
    //}
}
