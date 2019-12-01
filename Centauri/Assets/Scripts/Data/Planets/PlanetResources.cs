using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetResouces
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

    public PlanetResouces()
    {
        this.Iron = Random.Range(1, 100);
        this.Water = Random.Range(1, 100);
        this.Oil = Random.Range(1, 100);
        this.Copper = Random.Range(1, 100);
        this.Granite = Random.Range(1, 100);
    }

    public void RemoveResouces(float amount)
    {
        Iron -= amount;
        Water -= amount;
        Oil -= amount;
        Copper -= amount;
        Granite -= amount;
    }

    public bool IsEmpty()
    {
        if (Iron <= 0 &&
            Water <= 0 &&
            Oil <= 0 &&
            Copper <= 0 &&
            Granite <= 0)
            return true;

        return false;
    }
}
