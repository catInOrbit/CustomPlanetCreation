using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleKeplerOrbits;

public class RendezvousCalculator
{
    private GameObject player;
    private GameObject rendezvousTarget;

    public float thetaAngle;
    public Vector3 transferPosition;

    public GameObject Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public GameObject RendezvousTarget
    {
        get
        {
            return rendezvousTarget;
        }

        set
        {
            rendezvousTarget = value;
        }
    }

    public RendezvousCalculator(GameObject player, GameObject rendezvousTarget)
    {
        Player = player;
        RendezvousTarget = rendezvousTarget;
    }

    public RendezvousCalculator()
    { }

    public Vector3 CalculateRendezvousPoint(GameObject player, GameObject rendezvousTarget)
    {
        float playerToAtrractorRadius = Vector3.Distance( player.transform.position, player.GetComponent<KeplerOrbitMover>().AttractorSettings.AttractorObject.transform.position);
        float targetToAtrractorRadius = Vector3.Distance(rendezvousTarget.transform.position, rendezvousTarget.GetComponent<KeplerOrbitMover>().AttractorSettings.AttractorObject.transform.position);

        thetaAngle = Mathf.PI * (1 - Mathf.Sqrt((1 / 8) * Mathf.Pow((1 + playerToAtrractorRadius / targetToAtrractorRadius), 3)));
        thetaAngle = Mathf.Rad2Deg;

        float transferXCor = playerToAtrractorRadius * Mathf.Cos(thetaAngle);
        float transferYcor = playerToAtrractorRadius * Mathf.Sin(thetaAngle);

        return new Vector3(transferXCor, transferYcor);

    }
}
