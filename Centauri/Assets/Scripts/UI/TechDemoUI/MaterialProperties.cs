using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialProperties
{
    public Texture2D MainTexture { get; set; }
    public bool EnableNormalMap { get; set; }
    public float DiffusePercentage { get; set; }
    public enum LightMode
    {
        Off = 1,
        Vert = 2,
        Frag = 3
    }

    public float SpecularPercentage { get; set; }
    public float SpecularPower { get; set; }

    public bool EnableAmbient { get; set; }
    public float AmbientPercentage { get; set; }
    public float AmbientFactor { get; set; }

    public float BumpScale { get; set; }

    public Vector4 AtmosphereColor { get; set; }





}