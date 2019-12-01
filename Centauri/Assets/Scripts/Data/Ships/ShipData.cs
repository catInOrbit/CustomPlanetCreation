using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ShipWeightData;
using EnginesData;
using ShipResource;
using SimpleKeplerOrbits;
using UnityEngine.UI;

[System.Serializable]
class ShipData : MonoBehaviour
{
    //Public with instantiation

    public WeightData weightData = new WeightData();
    public RawResorces rawResorces = new RawResorces();
    public Fuel fuelData = new Fuel(1000, 1000);

    //Private with instantiation

    //Private 
    private KeplerOrbitMover keplerOrbitMover;
    private AtmosphericDrag atmosphericDrag;
    private GameObject currentOrbitingPlanet;
    private readonly AvailableEngines availableEngines = new AvailableEngines();
    private EngineData currentEngineData;
    private SpeedData speedData;
    //Props
    public WeightData WeightData
    {
        get
        {
            return weightData;
        }

        set
        {
            weightData = value;
        }
    }
    internal RawResorces RawResorces
    {
        get
        {
            return rawResorces;
        }

        set
        {
            rawResorces = value;
        }
    }

    public GameObject CurrentOrbitingPlanet
    {
        get
        {
            return currentOrbitingPlanet;
        }

        set
        {
            currentOrbitingPlanet = value;
        }
    }
    public AtmosphericDrag AtmosphericDrag
    {
        get
        {
            return atmosphericDrag;
        }

        set
        {
            atmosphericDrag = value;
        }
    }

    public EngineData CurrentEngineData
    {
        get
        {
            return currentEngineData;
        }

        set
        {
            currentEngineData = value;
        }
    }

    void Start()
    {
        keplerOrbitMover = this.GetComponent<KeplerOrbitMover>();
        CurrentOrbitingPlanet = keplerOrbitMover.AttractorSettings.AttractorObject.gameObject;
        AtmosphericDrag = CurrentOrbitingPlanet.GetComponentInChildren<AtmosphericDrag>();
        speedData = this.GetComponent<SpeedData>();

        CurrentEngineData = availableEngines.ListOfEngines["Vulkan"];
        WeightData.LoadResources(1000, fuelData.CurrentFuelAmount, 0);
    }

    void Update()
    {
        ThrustFuelConsumptionCalculation(speedData.GetEnginePercentageInteger(), CurrentEngineData.FuelConsumption);
    }

    private void ThrustFuelConsumptionCalculation(float engineThrustPower, float consumptionRate)
    {
        float currentRate = engineThrustPower / 100 * consumptionRate;
        if (fuelData.CurrentFuelAmount >= 0)
        {
            WeightData.ReCalculateWetMass(currentRate);
            fuelData.CurrentFuelAmount -= currentRate;
        }
        else
            fuelData.CurrentFuelAmount = 0;

       // Debug.Log(fuelData.CurrentFuelAmount);
        //Debug.Log("Mass" + WeightData.TotalMass);
    }
}
