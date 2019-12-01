using UnityEngine;
using System;
using System.Collections;

public class GraphicControl : MonoBehaviour 
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
    private Material planetMaterial;

    public GraphicControl(Material planetMaterial)
    {
        this.planetMaterial = planetMaterial;
    }
    
    public void EnableNormalSwitch()
    {
        if(planetMaterial.GetInt("_UseNormal") == 0)
            planetMaterial.SetInt("_UseNormal", 1);
        else
            planetMaterial.SetInt("_UseNormal", 0);
    }

    public void SetDiffusePercentage(float value)
    {
        planetMaterial.SetFloat("_Diffuse", value);
    }

    public void SetLightMode()
    {
        if(planetMaterial.GetInt("_Lighting") == 0)
            planetMaterial.SetInt("_Lighting", 1);

        else if(planetMaterial.GetInt("_Lighting") == 1)
            planetMaterial.SetInt("_Lighting", 2);
            
        else if(planetMaterial.GetInt("_Lighting") == 2)
            planetMaterial.SetInt("_Lighting", 0);
    }

    public void SetSpecularPercentage(float value)
    {
        planetMaterial.SetFloat("_SpecularFactor", value);
    }

    public void SetSpecularPower(float value)
    {
        planetMaterial.SetFloat("_SpecularPower", value);
    }

    public void EnableAmbientSwitch()
    {
        if(planetMaterial.GetInt("_AmbientMode") == 0 )
            planetMaterial.SetInt("_AmbientMode", 1);

        else
            planetMaterial.SetInt("_AmbientMode", 0);
    }

    public void ApplyNormalMap(Texture2D normalMap) => planetMaterial.SetTexture("_NormalMap", normalMap);

    public void ApplyHeightMap(Texture2D heightMap) => planetMaterial.SetTexture("_HeightMap", heightMap);

    public void ApplyTexture(Texture2D texture) => planetMaterial.SetTexture("_MainTex", texture);

    public void ApplyColor(Color color) => planetMaterial.SetColor("_Color", color);

    public void ApplyHeightMapHeight(float value) => planetMaterial.SetFloat("_HeightMapHeight", value);
    public void ApplySpecular(float value) => planetMaterial.SetFloat("_SpecularFactor", value);
    public void ApplySepcularPower(float value) => planetMaterial.SetFloat("_SpecularPower", value);
    public void ApplyAmbient(float value) => planetMaterial.SetFloat("_AmbientFactor", value);

    public void ApplyAtmosphereColor(Color color) => planetMaterial.SetColor("_AtmoColor", color);

    public void ApplyBumpScale(float value) => planetMaterial.SetFloat("_BumpScale", value);
}