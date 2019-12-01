using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUtils : MonoBehaviour
{
    public Vector2 ScreenProperties { get; set; }
    
    public ScreenUtils(float screenWidth, float screenHeight)
    {
        ScreenProperties = new Vector2(screenWidth, screenHeight);
    }

    public ScreenUtils()
    {
        
    }

     /// <summary>
    /// Return the float value of 1/4 of the screen.
    /// This float value is a hypotenuse of the triangle in 1/4 of screen
    /// </summary>
    /// <param name="width">The width of the screen</param>
    /// <param name="height">The height of the screen</param>
    public float GetScreenConstraint14(float width, float height)
    {
        return Mathf.Sqrt(Mathf.Pow(width, 2) + Mathf.Pow(height, 2)) / 4;
    }

      public float GetScreenConstraint14()
    {
        return Mathf.Sqrt(Mathf.Pow(ScreenProperties.x, 2) + Mathf.Pow(ScreenProperties.y, 2)) / 4;
    }

     /// <summary>
    /// Return the float value of 1/2 of the screen.
    /// This float value is a hypotenuse of the triangle in 1/2 of screen
    /// </summary>
    /// <param name="width">The width of the screen</param>
    /// <param name="height">The height of the screen</param>
    public Vector2 GetScreenHalfpoint(float width, float height)
    {
        return new Vector2(width / 2, height / 2);
    }

    public Vector2 GetScreenHalfpoint()
    {
        return new Vector2(ScreenProperties.x / 2, ScreenProperties.y / 2);
    }






}