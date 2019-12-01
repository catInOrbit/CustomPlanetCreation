using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace ShipWeightData
{
    [Serializable]
    public class WeightData
    {
        private float dryMass;
        private float wetMass;
        private float totalMass;

        public float TotalMass
        {
            get
            {
                return totalMass;
            }

            set
            {
                totalMass = value;
            }
        }

        public float DryMass
        {
            get
            {
                return dryMass;
            }

            set
            {
                dryMass = value;
            }
        }

        public float WetMass
        {
            get
            {
                return wetMass;
            }

            set
            {
                wetMass = value;
            }
        }

        public void LoadResources(float shipWeight, float fuel, float cargo)
        {
            DryMass = shipWeight + cargo;
            WetMass = shipWeight + fuel + cargo;
        }

        public float GetTotalMass()
        {
            TotalMass = DryMass + WetMass;
            return TotalMass;
        }

        public void ReCalculateWetMass(float fuel)
        {
            WetMass -= fuel;
        }

    }
}

