using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlanetSelectionUI : MonoBehaviour
{
    private float timer;
    private const float timerThreshold = 2;
    public bool IsSelected { get; set; }
    public string PlanetName { get; set; }
    private CameraMovement cameraMovement;

    private void Start()
    {
        cameraMovement = GameObject.FindObjectOfType<CameraMovement>();
    }

    void OnTouchStay()
    {
        if (!cameraMovement.IsMoving)
        {
            timer += Time.deltaTime;
            if (timer >= timerThreshold)
            {
                IsSelected = true;
                cameraMovement.CurrentCameraObject = this.gameObject;
                cameraMovement.IsFocusRequested = true;
            }
        }
    }

    void OnTouchUp()
    {
        SetFocusExitConditions();
    }

    private void SetFocusExitConditions()
    {
        timer = 0;
        cameraMovement.IsFocusRequested = false;
    }
}
