using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitLineRendererTest : MonoBehaviour
{

    public GameObject mainParentBody;

    private float h;
    private float k;

    public int res = 1000;
    public float theta;
    public float gConstant;
    public float angleTest;

    public float semiMajorAxis;
    public float semiMinorAxis;
    public float orbitPeriod;
    public float eccentricity;

    private Vector3[] positions;
    public Vector3 orbitCenter;
    private Quaternion q;
    private LineRenderer lineRenderer;

    private GravitationalPull gravityTest;
    public bool calculateOrbit = false;

    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        positions = new Vector3[res + 1];
        gravityTest = GameObject.FindObjectOfType<GravitationalPull>();

       
    }

    void Update()
    {
        //theta = Mathf.Atan2(this.transform.position.y, this.transform.position.x) * 180;
        q = Quaternion.AngleAxis(theta, Vector3.forward);
        //orbitCenter = new Vector2(this.transform.position.x - (semiMajorAxis * Mathf.Sin(orbitPeriod)), this.transform.position.y - (semiMinorAxis * Mathf.Cos(orbitPeriod)));  //new Vector3(mainParentBody.transform.position.x, mainParentBody.transform.position.y, 0);
        if (calculateOrbit)
        {
            OneTimeCalculation();
            //calculateOrbit = false;
        }
        Debug.DrawRay(this.transform.position, this.GetComponent<Rigidbody>().velocity, Color.red);

        for (int i = 0; i <= res; i++)
        {

            float angle = (float)i / (float)res * 2.0f * Mathf.PI;


            //semiMinorAxis = float.Parse(semiMajorAxis.ToString()) * Mathf.Sqrt((1 - 0.2f * 0.2f));


            positions[i] = new Vector3(semiMajorAxis * Mathf.Cos(angle), semiMinorAxis * Mathf.Sin(angle), 0);

            //orbitCenter = new Vector2(Mathf.Sin(angle) * semiMajorAxis, Mathf.Cos(angle) * semiMinorAxis);

            positions[i] = q * positions[i] + orbitCenter;

        }

        lineRenderer.positionCount = res + 1;
        lineRenderer.SetPositions(positions);
    }

    void OneTimeCalculation()
    {


        float radius = gravityTest.RadiusBetweenBody;

        orbitPeriod = 2 * Mathf.PI * Mathf.Sqrt(Mathf.Pow(radius, 3) / (gConstant * mainParentBody.transform.localScale.x));


        semiMajorAxis = Mathf.Pow(((Mathf.Pow(orbitPeriod, 2) * gConstant * (this.transform.localScale.x + mainParentBody.transform.localScale.x)) / (4 * Mathf.PI * Mathf.PI)), 1f / 3f);

        float standardGparameter = gConstant * (this.transform.localScale.x + mainParentBody.transform.localScale.x);
        float angularMomentum = radius * this.transform.localScale.x * (this.GetComponent<Rigidbody>().velocity.magnitude / radius);

        float alpha = gravityTest.Gravity * radius * radius;

        float orbitalEnergy = standardGparameter / (2 * semiMajorAxis);

        float reduceMass = (mainParentBody.transform.localScale.x + this.transform.localScale.x) / (mainParentBody.transform.localScale.x * this.transform.localScale.x);

        eccentricity = Mathf.Sqrt(1 + ((2 * orbitalEnergy * Mathf.Pow(angularMomentum, 2)) / (reduceMass * alpha * alpha)));

        semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - (eccentricity * eccentricity - 0.05f));

        orbitCenter = new Vector2((this.transform.position.x - mainParentBody.transform.position.x) / 2, (this.transform.position.y - mainParentBody.transform.position.y) / 2);

    }

    public static float SignedAngle(Vector3 from, Vector3 to, Vector3 normal)
    {
        // angle in [0,180]
        float angle = Vector3.Angle(from, to);
        float sign = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(from, to)));
        return angle * sign * 2;
    }
}
