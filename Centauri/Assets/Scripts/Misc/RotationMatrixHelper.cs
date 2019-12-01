using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RotationMatrixHelper
{
    private static bool LOG = false;

    // Instance variables------------------------------------------------

    /** The rotation matrix.  This is a 4x4 matrix. */
    private double[][] matrix;

    // The parameters for the rotation.
    /** x-coordinate of a point on the line of rotation. */
    private double a;

    /** y-coordinate of a point on the line of rotation. */
    private double b;

    /** z-coordinate of a point on the line of rotation. */
    private double c;

    /** x-coordinate of the line's direction vector. */
    private double u;

    /** y-coordinate of the line's direction vector. */
    private double v;

    /** z-coordinate of the line's direction vector. */
    private double w;

    // Some intermediate values...
    /** An intermediate value used in computations (u^2). */
    private double u2;
    private double v2;
    private double w2;
    private double cosT;
    private double sinT;
    private double l2;
    /** The length of the direction vector. */
    private double l;

    /** The 1,1 entry in the matrix. */
    private double m11;
    private double m12;
    private double m13;
    private double m14;
    private double m21;
    private double m22;
    private double m23;
    private double m24;
    private double m31;
    private double m32;
    private double m33;
    private double m34;


    // Constructor------------------------------------------------------
    public RotationMatrixHelper()
    {

    }


    /// <summary>
    ///Build a rotation matrix for rotations about the line through
    ///$P_1(a, b, c)$ parallel to $\langle u, v, w\rangle$ by the
    ///angle $\theta$.
    /// </summary>
    /// <param name="a"> x-coordinate of the center point of rotation</param>
    /// <param name="b"> y-coordinate of the center point of rotation</param>
    /// <param name="c"> z-coordinate of the center point of rotation</param>
    /// <param name="u"> x-coordinate of the line's direction vector.</param>
    /// <param name="v"> y-coordinate of the line's direction vector.</param>
    /// <param name="w"> z-coordinate of the line's direction vector.</param>
    /// <param name="theta">  The angle of rotation.</param>
    public void AcceptInput(double a,
                          double b,
                          double c,
                          double u,
                          double v,
                          double w,
                          double theta)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.u = u;
        this.v = v;
        this.w = w;
        // Set some intermediate values.
        u2 = u * u;
        v2 = v * v;
        w2 = w * w;
        cosT = Math.Cos(theta);
        sinT = Math.Sin(theta);
        l2 = u2 + v2 + w2;
        l = Math.Sqrt(l2);

        if (l2 < 0.000000001)
        {
            Console.WriteLine("RotationMatrix: direction vector too short!");
            return;             // Don't bother.
        }

        // Build the matrix entries element by element. 
        m11 = (u2 + (v2 + w2) * cosT) / l2;
        m12 = (u * v * (1 - cosT) - w * l * sinT) / l2;
        m13 = (u * w * (1 - cosT) + v * l * sinT) / l2;
        m14 = (a * (v2 + w2) - u * (b * v + c * w)
            + (u * (b * v + c * w) - a * (v2 + w2)) * cosT + (b * w - c * v) * l * sinT) / l2;

        m21 = (u * v * (1 - cosT) + w * l * sinT) / l2;
        m22 = (v2 + (u2 + w2) * cosT) / l2;
        m23 = (v * w * (1 - cosT) - u * l * sinT) / l2;
        m24 = (b * (u2 + w2) - v * (a * u + c * w)
            + (v * (a * u + c * w) - b * (u2 + w2)) * cosT + (c * u - a * w) * l * sinT) / l2;

        m31 = (u * w * (1 - cosT) - v * l * sinT) / l2;
        m32 = (v * w * (1 - cosT) + u * l * sinT) / l2;
        m33 = (w2 + (u2 + v2) * cosT) / l2;
        m34 = (c * (u2 + v2) - w * (a * u + b * v)
            + (w * (a * u + b * v) - c * (u2 + v2)) * cosT + (a * v - b * u) * l * sinT) / l2;

    }

    /// <summary>
    ///Multiply this matrix times the given coordinates (x, y, z, 1),
    ///representing a point P(x, y, z)..
    /// </summary>
    /// <param name="x">The point's x-coordinate which will rotate around defined fixed point.</param>
    /// <param name="y">The point's y-coordinate  which will rotate around defined fixed point.</param>
    /// <param name="z">The point's z-coordinate  which will rotate around defined fixed point.</param>
    /// <para>Return:</para> 
    ///<returns> The product, in a vector [#, #, #, 1], representing the
    /// rotated point.</returns>  
    public double[] TimesXYZ(double x, double y, double z)
    {
        double[] p = new double[4];
        p[0] = m11 * x + m12 * y + m13 * z + m14;
        p[1] = m21 * x + m22 * y + m23 * z + m24;
        p[2] = m31 * x + m32 * y + m33 * z + m34;
        p[3] = 1;

        return p;
    }

    /// <summary>
    ///ToString method for debug purposes
    /// </summary>
    /// <param name="inputs">doble[] inputs returned from method:</param>
    /// <example>
    /// <code>
    ///public double[] TimesXYZ(double x, double y, double z)
    /// </code>
    /// </example>

    public String TimeXYZToString(double[] inputs)
    {
        String returnResult = "";
        foreach (var item in inputs)
        {
            returnResult += "/ " + item.ToString();
        }

        return returnResult;
    }

    public double[] timesXYZ(double[] point)
    {
        return TimesXYZ(point[0], point[1], point[2]);
    }

    public static Vector2 CalculateVectorRotationalMatrix(float angle, Vector2 vector)
    {
        //xcosA + ysinA, -x sinA +ycosA
        return new Vector2( 0 + vector.y * Mathf.Sin(angle), -vector.x * Mathf.Sin(angle) + 0);
    }
}