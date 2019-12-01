using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGlitch : MonoBehaviour
{
    public float glitchAmount;
    public GameObject glitchImage;

    private void Update()
    {
        glitchImage.GetComponent<Image>().material.SetFloat("_GlitchEffect", glitchAmount);
    }
}
