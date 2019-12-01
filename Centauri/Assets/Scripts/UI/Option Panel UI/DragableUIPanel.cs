using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragableUIPanel : MonoBehaviour
{
    Vector3 moveVector;
    Vector3 moveVectorMouse;
    public GameObject uiCanvas;

    public void MovePanelWithInput()
    {
        Vector2 screenRes = new Vector2(Screen.width, Screen.height);
        float pythoScreenRes = Mathf.Sqrt(Mathf.Pow(screenRes.x, 2) + Mathf.Pow(screenRes.y, 2));
        Vector2 canvasRes = new Vector2(uiCanvas.GetComponent<RectTransform>().position.x, uiCanvas.GetComponent<RectTransform>().position.y);
        float pythoCanvasRes = Mathf.Sqrt(Mathf.Pow(canvasRes.x, 2) + Mathf.Pow(canvasRes.y, 2));

        float ratio = pythoCanvasRes / pythoScreenRes;

        Vector2 ratioVector = new Vector2(canvasRes.x / screenRes.x, canvasRes.y / screenRes.y);

        Vector2 centerPos = new Vector2(screenRes.x / 2, screenRes.y / 2);
        //Vector2 temp = RectTransformUtility.PixelAdjustPoint(mousePosActual, this.transform.parent.GetComponent<RectTransform>().transform, uiCanvas.GetComponent<Canvas>());
        if(Input.touchCount > 0)
        {
            Vector2 inputPos = new Vector2(Input.GetTouch(0).position.x - centerPos.x, Input.GetTouch(0).position.y - centerPos.y);
            this.GetComponent<RectTransform>().localPosition = inputPos;
        }

#if UNITY_EDITOR
        //Vector3 mousePosActual = new Vector2(Input.mousePosition.x - (centerPos.x), Input.mousePosition.y - (centerPos.y * 2));
        Vector3 mousePosActual = new Vector2(Input.mousePosition.x * ratioVector.x, Input.mousePosition.y * ratioVector.y);

        Vector2 actual;

        Vector3 objectPos = this.GetComponent<RectTransform>().localPosition;
        this.GetComponent<RectTransform>().localPosition = new Vector2(objectPos.x - mousePosActual.x, objectPos.y - mousePosActual.y);

        //Debug.Log(mousePosActual);
        //Debug.Log(ratio);
        //Debug.Log("Convert cord: " + new Vector3(mousePosActual.x, mousePosActual.y));
#endif
    }
}
