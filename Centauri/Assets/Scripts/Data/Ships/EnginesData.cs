using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnginesData
{
    [System.Serializable]
    public class EngineData
    {

        private float engineISP;
        private float thrustPower; //Exhaust velocity
        private string engineName;
        private float fuelConsumption;
        private float forceInputDampenent;

        public float EngineISP
        {
            get
            {
                return engineISP;
            }

            set
            {
                engineISP = value;
            }
        }

        public float ThrustPower
        {
            get
            {
                return thrustPower;
            }

            set
            {
                thrustPower = value;
            }
        }

        public string EngineName
        {
            get
            {
                return engineName;
            }

            set
            {
                engineName = value;
            }
        }

        public float FuelConsumption
        {
            get
            {
                return fuelConsumption;
            }

            set
            {
                fuelConsumption = value;
            }
        }

        public float ForceInputDampenent
        {
            get
            {
                return forceInputDampenent;
            }

            set
            {
                forceInputDampenent = value;
            }
        }

        public EngineData(float engineISP, float thrustPower, string engineName, float fuelConsumption, float forceInputDampenent)
        {
            EngineISP = engineISP;
            ThrustPower = thrustPower;
            EngineName = engineName;
            FuelConsumption = fuelConsumption;
            ForceInputDampenent = forceInputDampenent;
        }

        public float GetExhaustVelocity()
        {
            return 9.8f * engineISP;
        }

        public float GetThrustWeightRatio(float exhaustVelocity, float shipDryMass, float gravity)
        {
            return exhaustVelocity / (shipDryMass * gravity);
        }
    }

    [System.Serializable]
    public class AvailableEngines
    {
        private readonly Dictionary<string, EngineData> listOfEngines = new Dictionary<string, EngineData>()
     {
        {"Vulkan", new EngineData(100, 1.6f, "Vulkan", 12f, 0.0001f) },
        {"Vulkan Atmosphere", new EngineData(200, 2.3f, "Vulkan Atmosphere", 24f, 0.0004f) },
    };

        public Dictionary<string, EngineData> ListOfEngines
        {
            get
            {
                return listOfEngines;
            }
        }
    }


}
