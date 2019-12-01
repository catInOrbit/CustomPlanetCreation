using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCam : MonoBehaviour
{
    public GameObject gameObject;
    [Range(0, 10)]
    public float sliderx, slidery, sliderz;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(gameObject.transform.position.x - sliderx, gameObject.transform.position.y - slidery, gameObject.transform.position.z - sliderz);
    }
}
