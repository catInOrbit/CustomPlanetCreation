using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VelocityVector : MonoBehaviour
{
    public GameObject velocity;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

	void Update ()
    {
        DrawVelocityVector();
    }

    private void DrawVelocityVector()
    {
        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, velocity.transform.position);
    }
}
