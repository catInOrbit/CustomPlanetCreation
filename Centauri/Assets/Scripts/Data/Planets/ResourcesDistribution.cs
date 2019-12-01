using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ResourcesDistribution : MonoBehaviour
{

    //Current method is to generate a cirlce at center (0,0), add all points as child of an object,
    //and move object to center of planet
    public Transform pointsParent;  //--> Object to move to center


    public int accuracy;
    public GameObject player;
    public Vector3[] spawnPoints;
    public GameObject resourcePoint;

    private GameObject surfacePoint;
    private List<Vector3> points = new List<Vector3>();

    private Dictionary<string, PlanetResouces> pointsAndResources = new Dictionary<string, PlanetResouces>();
    //private int[] previousRandomPoints;

    public List<Vector3> Points
    {
        get
        {
            return points;
        }

        set
        {
            points = value;
        }
    }
    public Dictionary<string, PlanetResouces> PointsAndResources
    {
        get
        {
            return pointsAndResources;
        }

        set
        {
            pointsAndResources = value;
        }
    }

    private void Start()
    {
        string searchString = string.Format("/{0}/{1}", this.name, "Surface Point");
        surfacePoint = GameObject.Find(searchString);

        HelperGenerateCirclePoints(accuracy);

        //Spawn resource points inside parent first
        for(int i = 0; i < 5; i++)
        {
            resourcePoint.name = "Resource point " + i;
            Thread.Sleep(10);
            spawnPoints = SpawnPoints(100, 10);
            InstantiatePoint(resourcePoint, spawnPoints, pointsParent);

            pointsAndResources.Add(resourcePoint.name + "(Clone)", new PlanetResouces());
        }
        
        //And move parent to center of planet
    }

    private void Update()
    {
        pointsParent.transform.position = this.transform.position;
    }

    private void HelperGenerateCirclePoints(int accuracy)
    {
        float radius =this.GetComponent<SphereCollider>().radius;

        for(int i = 0; i <= accuracy; i++)
        {
            float angle = (float)i / (float)accuracy * 2.0f * Mathf.PI;
            Points.Add(new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0f));
        }
    }

    public Vector3[] SpawnPoints(int accuracy, int spawnRange)
    {
        int randomPoint = Random.Range(0, accuracy - 10);
        int randPointPlus = 0;

        if ((randomPoint + spawnRange) < accuracy)
            randPointPlus = randomPoint + spawnRange; //The amount of spawn point

        Vector3[] returnList = new Vector3[randPointPlus];

        for (int i = randomPoint; i < randPointPlus; i++)
            returnList[i] = Points[i];

        return returnList;
    }

    private void InstantiatePoint(GameObject point, Vector3[] positions, Transform parent)
    {
        foreach (Vector3 position in positions)
            Instantiate(point, position, Quaternion.identity, parent.transform);
    }


    public bool PlayerIsInResourcesZone(GameObject player, Vector3[] points)
    {
      if((player.transform.position.x >= points[0].x && player.transform.position.x <= points[points.Length - 1].x) ||
            (player.transform.position.x <= points[0].x && player.transform.position.x >= points[points.Length - 1].x))
            return true;

        return false;
    }

    public PlanetResouces GetValueFromDictionary(string keyName)
    {
        return PointsAndResources[keyName];
    }


    

}
