using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomUI : MonoBehaviour
{
    private CameraMovement cameraMovement;

    private void Start()
    {
        cameraMovement = Camera.main.gameObject.GetComponent<CameraMovement>();
    }

    public void CameraAutoZoom()
    {
        cameraMovement.EnableAutoZoom = true;
    }

    public void PinchZoom()
    {
        cameraMovement.EnableAutoZoom = false;
    }
}
