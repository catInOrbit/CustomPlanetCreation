using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class OnScreenGUI : MonoBehaviour
{
    public Sprite[] dragGUIS = new Sprite[2];

    private InputToForceUtils inputToForceUtils = new InputToForceUtils();
    private InputDetectionUtils inputDetectionUtils = new InputDetectionUtils();
    private GameObject canvas;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    private void Update()
    {
        //ChangeSpriteWithSpeed();
        if (inputDetectionUtils.ProcessTouchStateOnePoint() == "Moving" || inputDetectionUtils.ProcessTouchStateOnePoint() == "Begin")
        {
            AttachToTouchPosition();
        }
#if UNITY_EDITOR
        if(Input.GetMouseButton(0))
        {
            AttachToTouchPosition();
        }
#endif
    }

    /// <summary>
    /// Transform touch position to Recttransform position, apply positional update to Image
    /// </summary>
    private void AttachToTouchPosition()
    {
        Vector2 uiOffset = new Vector2(canvas.GetComponent<RectTransform>().sizeDelta.x / 2f, canvas.GetComponent<RectTransform>().sizeDelta.y / 2f);
        Vector2 viewportPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 proportionalPosition = new Vector2(viewportPoint.x * canvas.GetComponent<RectTransform>().sizeDelta.x, viewportPoint.y
                                                                                                                       * canvas.GetComponent<RectTransform>().sizeDelta.y);

        this.GetComponent<RectTransform>().anchoredPosition = proportionalPosition - uiOffset;
        }


    /// <summary>
    /// Apply appropriate sprite indication when drag force changes
    /// </summary>
    private void ChangeSpriteWithSpeed(Sprite sprite) //TODO: Implement
    {
        //if(inputDetectionUtils.ProcessTouchStateOnePoint() == "Moving" || inputDetectionUtils.ProcessTouchStateOnePoint() == "Begin")
        {
            float force = inputDetectionUtils.TouchStartPos.magnitude * inputToForceUtils.DragToForceTranslation(Screen.width, Screen.height);
            if (force > 25)
                sprite = dragGUIS[0];
            if(force > 50)
                sprite = dragGUIS[1];
        }
    }
}