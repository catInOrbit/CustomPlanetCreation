using System.Collections;
using System.Collections.Generic;
using SimpleKeplerOrbits;
using UnityEngine;

public class RendezvousDisplay : MonoBehaviour
{

    public GameObject playerInstance;
    public GameObject rendezvousTarget;
    public Vector3 rdPoint;

    private RendezvousCalculator rendezvousCalculator = new RendezvousCalculator();

    public bool plot;

	void Update ()
    {
		if(plot == true)
        {
            rdPoint = rendezvousCalculator.CalculateRendezvousPoint(this.gameObject, rendezvousTarget);
            playerInstance.GetComponent<KeplerOrbitMover>().isStaticInstance = true;
            playerInstance.GetComponent<KeplerOrbitMover>().AttractorSettings.AttractorObject = this.GetComponent<KeplerOrbitMover>().AttractorSettings.AttractorObject;
           
            Instantiate(playerInstance,rdPoint, Quaternion.identity);
            plot = false;
        }

    }
}
