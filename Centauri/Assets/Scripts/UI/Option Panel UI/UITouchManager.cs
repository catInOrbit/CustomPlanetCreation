using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UITouchManager : MonoBehaviour
{
    public GameObject mainUI;
    //public BoxCollider touchZone;
    public GameObject[] uiControls;
    public GameObject cancelUI;

    [Tooltip("UI that is currently running in (displaying panel)")]
    private bool displayUIOptions;
    private bool cancelBool;

    private Queue<bool> activationMessageQueue = new Queue<bool>();

    //Dictionary of uis and their index in uiCOntrols
    private List<GameObject> activeUI = new List<GameObject>();
    private GameObject currentUI;
    private CameraMovement cameraMovement;

    Vector3 firstTouchPos;
    Vector3 secondTouchPos;

    private bool isTouchZone;
    private Vector3 mouseStartPos;
    private Vector3 mouseEndpos;
    private Vector3 mouseMoveDirection;
    public bool CancelBool
    {
        get
        {
            return cancelBool;
        }

        set
        {
            cancelBool = value;
        }
    }

    public bool DisplayUIOptions
    {
        get
        {
            return displayUIOptions;
        }

        set
        {
            displayUIOptions = value;
        }
    }

    public Queue<bool> ActivationMessageQueue
    {
        get
        {
            return activationMessageQueue;
        }

        set
        {
            activationMessageQueue = value;
        }
    }
    private void Start()
    {
        cameraMovement = GameObject.FindObjectOfType<CameraMovement>();
    }

    void Update ()
    {

        if(ActivationMessageQueue.Count == 1)
        {
            cameraMovement.UiGainControl = true;
            ActivateUIOptions();
        }

        if(activationMessageQueue.Count == 2)
        {
            cameraMovement.UiGainControl = false;
            if (activeUI.Count <= 0)
                DeactivateAllUI();
            else if (activeUI.Count > 0)
            {
                DeactiavateInactiveUI();
            }

            ActivationMessageQueue.Clear();
        }

        if(CancelBool == true)
            DeactivateAllUI();

        MoveUIWithPlayer();
    }


//    private void SwitchUIOption(string direction)
//    {
//        if (direction.Equals("Right"))
//        {
//            for (int i = 0; i < uiControls.Length; i++)
//            {
//                if (uiIndexsDictionary[i].gameObject.activeSelf == false)   
//                {
//                    currentIndex = i;
//                    break;
//                }
//            }
//        }
//    }

//    private void SwipeVectorHelperMethod()
//    {
//        if (Input.touchCount > 0)
//        {
//            Touch touch = Input.GetTouch(0);

//            switch (touch.phase)
//            {
//                case TouchPhase.Began:
//                    firstTouchPos = touch.position;
//                    break;
//                case TouchPhase.Ended:
//                    secondTouchPos = touch.position;
//                    Vector3 swipeDirection = secondTouchPos - firstTouchPos;

//                    if (swipeDirection.x > 0)
//                        SwitchUIOption("Right");
//                    if (swipeDirection.x < 0)
//                        SwitchUIOption("Left");
//                    break;
//            }
//        }

//#if UNITY_EDITOR
//        if (Input.GetMouseButtonDown(0))
//            mouseStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//        if (Input.GetMouseButtonUp(0))
//        {
//            mouseEndpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//            mouseMoveDirection = mouseEndpos - mouseStartPos;
//            if (mouseMoveDirection.x > 0)
//                SwitchUIOption("Left");
//            else if (mouseMoveDirection.x < 0)
//                SwitchUIOption("Right");
//        }
//#endif
//    }

    private void MoveUIWithPlayer()
    {
        mainUI.GetComponent<RectTransform>().localPosition = Camera.main.WorldToViewportPoint(this.transform.parent.transform.position);
    }

    //public void SpawnPanel(GameObject parentObject)
    //{
    //    string searchString = string.Format("/Canvas/UI Selection/{0}/{1}", parentObject.name, parentObject.name + " Panel");
    //    GameObject animationPanel = GameObject.Find(searchString);

    //    string prefabSearchString = parentObject.gameObject.name + " Panel Prefab";
    //    Instantiate(Resources.Load("Speed Panel Prefab"), animationPanel.GetComponent<RectTransform>().position, Quaternion.identity, panelSpawnPos.transform);
    //}

    private void RestartTouchUI()
    {
        cameraMovement.UiGainControl = false;
    }

    public void DeactivateOtherUI(GameObject thisUI)
    {
        activeUI.Add(thisUI.gameObject);
        for (int i = 0; i < uiControls.Length; i++)
        {
            if (!activeUI.Contains(uiControls[i].gameObject))
                uiControls[i].gameObject.SetActive(false);
        }

        RestartTouchUI();
    }

    private void ActivateUIOptions()
    {
        for (int i = 0; i < uiControls.Length; i++)
        {
            if (!activeUI.Contains(uiControls[i].gameObject))
                uiControls[i].gameObject.SetActive(true);
        }
    }

    private void DeactivateAllUI()
    {
        for (int i = 0; i < uiControls.Length; i++)
            uiControls[i].gameObject.SetActive(false);
    }

    private void DeactiavateInactiveUI()
    {
        for (int i = 0; i < uiControls.Length; i++)
        {
            if (!activeUI.Contains(uiControls[i].gameObject))
                uiControls[i].gameObject.SetActive(false);
        }
    }

    public void RemoveFromActiveUIList(GameObject thisUI)
    {
        activeUI.Remove(thisUI);
    }
}
