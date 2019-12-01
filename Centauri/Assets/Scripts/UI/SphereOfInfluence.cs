using SimpleKeplerOrbits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereOfInfluence : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
            other.gameObject.GetComponent<KeplerOrbitMover>().AttractorSettings = this.transform.parent.GetComponent<AttractorData>();
    }
}
