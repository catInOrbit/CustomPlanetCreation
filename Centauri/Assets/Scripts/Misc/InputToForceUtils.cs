using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///This class translates the magnitude of swipe (touch start pos to touch end pos) to in-game force
/// </summary>
public class InputToForceUtils
{
    private int addedDistance;
    public int AddedDistance
    {
        get
        {
            return addedDistance;
        }
        set
        {
            addedDistance = 1;
        }
    }

    /// <summary>
    ///Use pythagorean formula for 1/4 screen plus an extra distance
    /// </summary>
    ///
    /// <param name="screenWidth">Width of screen</param>
    /// <param name="screenHeight">Height of screen</param>
    private float CalculateDesirableScreenDragDistance(float screenWidth, float screenHeight)
    {
        float a = screenWidth / 2;
        float b = screenHeight / 2;

        return Mathf.Sqrt((a*a) +(b * b)) + addedDistance;
    }

    /// <summary>
    ///Max drag distance will be capped at 100%
    /// </summary>
    /// <param name="screenWidth">Width of screen</param>
    /// <param name="screenHeight">Height of screen</param>
    /// <remarks>
    /// <para>Depended function:</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// private float CalculateDesirableScreenDragDistance(float screenWidth, float screenHeight)
    ///{
    ///    float a = screenWidth / 2;
    ///    float b = screenHeight / 2;
    ///    return Mathf.Sqrt((a*a) +(b * b)) + addedDistance;
    ///}
    /// </code>
    /// </example>
    public float DragToForceTranslation(float screenWidth, float screenHeight)
    {
        return CalculateDesirableScreenDragDistance(screenWidth, screenHeight) / 100;
    }
}