using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Test : MonoBehaviour
{

    public float a = 5;
    public float b = 3;
    public float h = 1;
    public float k = 1;
    public float theta = 45;
    public int resolution = 1000;

    public float distanceBtFocus;
    public float focuesAngle;
    public float centerX;
    public float centerY;

    public float initialVelocity;
    public float angle;
    public float time;
    public float gravity;



    public GameObject locus1;
    public GameObject locus2;
    public Vector3 center;


    private Vector3[] positions;

    void Update()
    {
        FO();
        positions = CreateEllipse2(); //CreateEllipse(a, b, h, k, theta, resolution);
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.SetVertexCount(resolution + 1);
        for (int i = 0; i <= resolution; i++)
        {
            lr.SetPosition(i, positions[i]);
        }
    }

    Vector3[] CreateEllipse(float a, float b, float h, float k, float theta, int resolution)
    {

        positions = new Vector3[resolution + 1];
        Quaternion q = Quaternion.AngleAxis(theta, Vector3.forward);
        //Vector3 center = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);

        for (int i = 0; i <= resolution; i++)
        {
            float angle = (float)i / (float)resolution * 2.0f * Mathf.PI;
            positions[i] = new Vector3( a * Mathf.Cos(angle),  b * Mathf.Sin(angle), 0.0f);
            positions[i] = q * positions[i] + center;
        }

        return positions;
    }

    public void FO()
    {
        distanceBtFocus = Vector2.Distance(locus1.transform.position, locus2.transform.position);
        focuesAngle = Mathf.Atan2(locus2.transform.position.x - locus1.transform.position.x, locus2.transform.position.y - locus1.transform.position.y);
        center = new Vector3(locus1.transform.position.x + locus2.transform.position.x, locus1.transform.position.y + locus2.transform.position.y, 0);

        b = Mathf.Sqrt(a * a - distanceBtFocus * distanceBtFocus/ 4);
    }

    Vector3[] CreateEllipse2()
    {
        positions = new Vector3[resolution + 1];
        for (int i = 0; i <= resolution; i++)
        {
            //float angle = (float)i / (float)resolution * 2.0f * Mathf.PI;
            //time += 0.1f;
            positions[i] = new Vector3(initialVelocity * Mathf.Cos(angle) * time, initialVelocity * Mathf.Sin(angle) - 0.5f * gravity * time * time, 0.0f);
            //positions[i] = q * positions[i] + center;
        }

        return positions;
    }

}