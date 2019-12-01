using SimpleKeplerOrbits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlanetResources : MonoBehaviour
{

    public bool beginResouceTransfer;


    private ResourcesDistribution resourcesDistribution;
    private ShipData shipData;
    private KeplerOrbitMover keplerOrbitMover;

    private GameObject currentOrbitingPlanet;
    string keyName = null;

    private void Start()
    {
        keplerOrbitMover = this.GetComponent<KeplerOrbitMover>();
        currentOrbitingPlanet = keplerOrbitMover.AttractorSettings.AttractorObject.gameObject;
        shipData = GameObject.FindObjectOfType<ShipData>();
        resourcesDistribution = currentOrbitingPlanet.GetComponent<ResourcesDistribution>();
    }

    private void Update()
    {
        if(beginResouceTransfer == true)
            GetResouceFromPlanet(keyName, 1, 0.3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Resource point "))
        {
            keyName = other.gameObject.name;
            beginResouceTransfer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Resource point "))
            beginResouceTransfer = false;
    }


    //Main method for extracting resources
    public void GetResouceFromPlanet(string keyFromPair, float transferSpeed, float transferTimeFrame)
    {

        PlanetResouces resoucesInstance = resourcesDistribution.PointsAndResources[keyFromPair];

        if(resoucesInstance.IsEmpty() == false)
        {
            StartCoroutine(WaitTimeTillTransfer(1));
            StartCoroutine(ExtractResouces(resoucesInstance, transferSpeed));
        }
    }

    IEnumerator WaitTimeTillTransfer(float transferTimeFrame)
    {
        float timer = 0;
        timer += Time.deltaTime;
        if (timer >= transferTimeFrame)
        {
            timer = 0;
            yield return null;
        }
    }

    IEnumerator ExtractResouces(PlanetResouces resoucesInstance, float transferSpeed)
    {
        shipData.RawResorces.AddResouceData(transferSpeed);
        resoucesInstance.RemoveResouces(transferSpeed);
        yield return null;
    }

}
