using SimpleKeplerOrbits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SimpleOrbitDataDisplay : MonoBehaviour
{

    public GameObject apoapsis; //Set in inspector
    public GameObject periapsis;//Set in inspector

    public GameObject orbitDataRectParent;

    [Header("Scale text with screen size")]
    public float textScaleAmount;

    private KeplerOrbitMover keplerOrbitMover;
    private GameObject tracking;
    string apoapsisName = "";
    string periapsisName = "";

    private void Start()
    {

        keplerOrbitMover = GetComponent<KeplerOrbitMover>();

        if (apoapsis != null && periapsis != null)
        {
            apoapsis.name = this.name + "_Apoapsis";
            apoapsisName = this.name + "_Apoapsis";
            Instantiate(apoapsis, orbitDataRectParent.transform);
            periapsis.name = this.name + "_Periapsis";
            periapsisName = this.name + "_Periapsis";
            Instantiate(periapsis, orbitDataRectParent.transform);

            string searchStringApoapsis = string.Format("/{0}/{1}(Clone)", orbitDataRectParent.name, apoapsisName).Trim();
            apoapsis = GameObject.Find(searchStringApoapsis);
            string searchStringPeriapsis = string.Format("/{0}/{1}(Clone)", orbitDataRectParent.name, periapsisName).Trim();
            periapsis = GameObject.Find(searchStringPeriapsis);
        }
    }

    private void Update()
    {
        ShowSimpleOrbitData(new Vector3((float)keplerOrbitMover.OrbitData.Apoapsis.x, (float)keplerOrbitMover.OrbitData.Apoapsis.y, 0),
            new Vector3((float)keplerOrbitMover.OrbitData.Periapsis.x, (float)keplerOrbitMover.OrbitData.Periapsis.y, 0));
    }

    private void LateUpdate()
    {
        ScaleTextWithScreen(textScaleAmount);
        FaceCamera();
    }

    private void ShowSimpleOrbitData(Vector3 apoapsisObject, Vector3 periapsisObject)
    {
        apoapsis.GetComponent<TextMesh>().text = string.Format("Apoapsis \r\n {0}", keplerOrbitMover.OrbitData.ApoapsisDistance.ToString("0.00"));
        periapsis.GetComponent<TextMesh>().text = string.Format("Periapsis \r\n {0}", keplerOrbitMover.OrbitData.PeriapsisDistance.ToString("0.00"));

        Vector3 apPos = keplerOrbitMover.AttractorSettings.AttractorObject.position + apoapsisObject;
        Vector3 periPos = keplerOrbitMover.AttractorSettings.AttractorObject.position + periapsisObject;

        if (apoapsisObject.x != Vector3.positiveInfinity.x)
            apoapsis.transform.localPosition = new Vector3(apPos.x, apPos.y, -4.5f); //Move apoapsis in front of any object

        if(periapsisObject.x != Vector3.positiveInfinity.x)
            periapsis.transform.localPosition = new Vector3(periPos.x, periPos.y, -4.5f); //Move periapsis in front of any object
    }

    private void ScaleTextWithScreen(float scaleAmount)
    {
        apoapsis.GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(Camera.main.orthographicSize * scaleAmount);
        periapsis.GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(Camera.main.orthographicSize * scaleAmount);
    }

    private void FaceCamera()
    {
        apoapsis.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        periapsis.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
