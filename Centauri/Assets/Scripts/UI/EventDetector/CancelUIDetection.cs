using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CancelUIDetection : MonoBehaviour
{
    private UITouchManager uiTouchManager;
    private OrbitalManeuver orbitalManeuver;


    private void Start()
    {
        orbitalManeuver = GameObject.FindObjectOfType<OrbitalManeuver>();
        uiTouchManager = GameObject.FindObjectOfType<UITouchManager>();
    }

    void OnTouchDown()
    {
        SendDeactivateUISignal();
    }

    void SendDeactivateUISignal()
    {

        uiTouchManager.enabled = true;
        uiTouchManager.CancelBool = true;
        uiTouchManager.DisplayUIOptions = false;
        orbitalManeuver.enabled = true;
    }
}
