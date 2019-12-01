using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManeuverBubble : MonoBehaviour
{
    private Transform player;

    private void Start() 
    {
        player = GameObject.Find("Player").transform;
    }
    private void Update() 
    {
        this.transform.position = player.transform.position;
    }
}