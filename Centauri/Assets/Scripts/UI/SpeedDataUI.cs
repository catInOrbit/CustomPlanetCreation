using SimpleKeplerOrbits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpeedDataUI : MonoBehaviour
{
    public Text deltaVText, velocityText, accelerationText, thrustPercentage;

    private SpeedData speedData;
    private OrbitalManeuver orbitalManeuver;

    private void Start()
    {
        speedData = GameObject.FindObjectOfType<SpeedData>();
        orbitalManeuver = this.GetComponent<OrbitalManeuver>();
    }

    private void Update()
    {
        AssignText(deltaVText, velocityText, accelerationText, thrustPercentage);
    }

    private void AssignText(Text deltaVText, Text velocityText, Text accelerationText, Text thrustPercentage)
    {

        deltaVText.text = speedData.GetDeltaVStringData();
        velocityText.text = speedData.GetVelocityStringData();
        accelerationText.text = speedData.GetAccelerationStringData();
        thrustPercentage.text = speedData.GetEngineThrustPercentage();
    }
}
