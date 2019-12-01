using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Button))]
public class BlockSelection : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isSelected = false;
    private PlayerUIDetection playerUIDetection;
    private PlanetSelectionUI planetSelectionUI;
    void Start()
    {
        playerUIDetection = GameObject.FindObjectOfType<PlayerUIDetection>();
        planetSelectionUI = GameObject.FindObjectOfType<PlanetSelectionUI>();
    }

    public void SetButtonState()
    {
        if(isSelected == true)
            isSelected = false;
        else
            isSelected = true;

        SetSelectionScriptsStatus();
        
    }

    private void SetSelectionScriptsStatus()
    {
        if(isSelected == true)
        {
            playerUIDetection.enabled = false;
            planetSelectionUI.enabled = false;
        }

        else
        {
            playerUIDetection.enabled = true;
            planetSelectionUI.enabled = true;
        }
    }
}