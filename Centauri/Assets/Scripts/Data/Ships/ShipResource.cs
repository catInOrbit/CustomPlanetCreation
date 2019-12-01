using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipResource
{
    class RawResorces
    {
        private float iron;
        private float water;
        private float oil;
        private float copper;
        private float granite;

        public float Iron
        {
            get
            {
                return iron;
            }

            set
            {
                iron = value;
            }
        }

        public float Water
        {
            get
            {
                return water;
            }

            set
            {
                water = value;
            }
        }

        public float Oil
        {
            get
            {
                return oil;
            }

            set
            {
                oil = value;
            }
        }

        public float Copper
        {
            get
            {
                return copper;
            }

            set
            {
                copper = value;
            }
        }

        public float Granite
        {
            get
            {
                return granite;
            }

            set
            {
                granite = value;
            }
        }

        public void AddResouceData(float amount)
        {
            Iron += amount;
            water += amount;
            oil += amount;
            copper += amount;
            granite += amount;
        }
    }

    class BuildingMaterial
    {
    }

    class Fuel
    {
        private float currentFuelAmount;
        private float tankCapaicty;

        public float CurrentFuelAmount
        {
            get
            {
                return currentFuelAmount;
            }

            set
            {
                currentFuelAmount = value;
            }
        }
        public float TankCapaicty
        {
            get
            {
                return tankCapaicty;
            }

            set
            {
                tankCapaicty = value;
            }
        }

        public Fuel(float currentFuelAmount, float tankCapaicty)
        {
            CurrentFuelAmount = currentFuelAmount;
            TankCapaicty = tankCapaicty;
        }


    }

}

