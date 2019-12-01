using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class CurrentOverallStatus : MonoBehaviour
{
    //Public
    public Image statusBar;

    //Private
    private ShipData shipData;
    private OnGroundDetection onGroundDetection;

    private Dictionary<string, Color> statusColor = new Dictionary<string, Color>()
    {
        { "Nominal", new Color32(0, 255, 144, 255) },
        { "Danger", new Color32(255, 4, 0, 255) },
        {"Landed", new Color32(0, 192, 255, 255) }
    };
    private Text statusText;


    private void Start()
    {
        shipData = this.GetComponent<ShipData>();
        statusText = statusBar.transform.GetChild(0).GetComponent<Text>();
        onGroundDetection = GameObject.FindObjectOfType<OnGroundDetection>();
    }

    private void Update()
    {
        DebugDisplayStatus();
    }

    public void DebugDisplayStatus()
    {
        if (shipData.AtmosphericDrag.EnableDrag == true)
        {
            statusBar.color = statusColor["Danger"];
            statusText.text = "Atmospheric Entry";
            statusText.color = statusColor["Danger"];
        }

        if(onGroundDetection.isLanded == true)
        {

            statusBar.color = statusColor["Landed"];
            statusText.text = "Landed";
            statusText.color = statusColor["Landed"];
        }

        if(shipData.AtmosphericDrag.EnableDrag == false && onGroundDetection.isLanded == false)
        {
            statusBar.color = statusColor["Nominal"];
            statusText.text = "Nominal";
            statusText.color = statusColor["Nominal"];
        }
    }
}
