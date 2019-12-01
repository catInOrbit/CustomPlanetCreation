using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BoundingBox : MonoBehaviour
{
    public String State{ get; set; }

    void OnTouchStay()
    {
        State = "InBound";
    }

    void OnTouchUp()
    {
        State = "End";
    }

    void OnTouchExit()
    {
        State = "OutBound";
    }
}
