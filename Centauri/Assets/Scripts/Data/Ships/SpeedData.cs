using SimpleKeplerOrbits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EnginesData;

[Serializable]
class SpeedData : MonoBehaviour
{
    private string deltaV, velocity, acceleration;
    private KeplerOrbitMover dataClass;
    private ShipData shipData;
    private OrbitalManeuver orbitalManeuver;

    void Awake()
    {
        dataClass = GameObject.FindGameObjectWithTag("Player").GetComponent<KeplerOrbitMover>();
    }

    private void Start()
    {
        shipData = GameObject.FindObjectOfType<ShipData>();
        orbitalManeuver = this.GetComponent<OrbitalManeuver>();
    }

    public string GetVelocityStringData()
    {
        velocity = dataClass.OrbitData.Velocity.magnitude.ToString("0.00") + " m/s";
        return velocity;
    }

    public string GetAccelerationStringData()
    {
        acceleration = (Mathf.Pow((float)dataClass.OrbitData.Velocity.magnitude, 2) / (float)dataClass.OrbitData.SemiMajorAxis).ToString("0.00") + " m/s";
        return acceleration;
    }

    public string GetDeltaVStringData()
    {
        deltaV = CalculateGetDeltaV(shipData.CurrentEngineData.GetExhaustVelocity(), shipData.WeightData.GetTotalMass(), shipData.WeightData.DryMass).ToString("0.00") + " m/s";

        return deltaV;
    }

    public string GetEngineThrustPercentage()
    {
        return CalculateEngineThrustPercentage(orbitalManeuver.ForceStrength, orbitalManeuver.VectorMagnitudeCap).ToString() + "%";
    }

    public int GetEnginePercentageInteger()
    {
        return CalculateEngineThrustPercentage(orbitalManeuver.ForceStrength, orbitalManeuver.VectorMagnitudeCap);
    }

    private float CalculateGetDeltaV(float exhaustVelocity, float shipFullMass, float shipEmptyMass)
    {
        return exhaustVelocity * Mathf.Log10(shipFullMass / shipEmptyMass);
    }

    public int CalculateEngineThrustPercentage(float inputVectorForceStrength, float vectorMagnitudeCap)
    {
        return Mathf.RoundToInt((inputVectorForceStrength / vectorMagnitudeCap) * 100);
    }

}
