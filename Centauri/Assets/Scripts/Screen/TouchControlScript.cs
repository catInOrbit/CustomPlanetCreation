using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlScript : MonoBehaviour
{
    public int[] layerMasksIndex;
    private readonly int acceptedLayer = 1 << 9;

    private void Start()
    {
        
    }

    void Update ()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            foreach (int layerIndex in layerMasksIndex)
            {
                int acceptedLayer = 1 << layerIndex;
                if (Physics.Raycast(ray, out hit, 10000, acceptedLayer))
                {
                    GameObject touchUI = hit.transform.gameObject;

                    if (touchUI.gameObject.layer != 0)
                    {
                        if (Input.GetMouseButtonDown(0))
                            touchUI.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);

                        if (Input.GetMouseButtonUp(0))
                            touchUI.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);

                        if (Input.GetMouseButton(0))
                            touchUI.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
#endif
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);
                RaycastHit raycastHit;
                foreach (int layerIndex in layerMasksIndex)
                {
                    int acceptedLayer = 1 << layerIndex;

                    if (Physics.Raycast(ray, out raycastHit, 10000, acceptedLayer))
                    {
                        GameObject touchUI = raycastHit.transform.gameObject;
                        if (touchUI.layer != 0)
                        {
                            if (touch.phase == TouchPhase.Began)
                                touchUI.SendMessage("OnTouchDown", raycastHit.point, SendMessageOptions.DontRequireReceiver);

                            if (touch.phase == TouchPhase.Ended)
                                touchUI.SendMessage("OnTouchUp", raycastHit.point, SendMessageOptions.DontRequireReceiver);

                            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                                touchUI.SendMessage("OnTouchStay", raycastHit.point, SendMessageOptions.DontRequireReceiver); //Press and hold

                            if (touch.phase == TouchPhase.Canceled)
                                touchUI.SendMessage("OnTouchExit", raycastHit.point, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
            }
        }
	}
}
