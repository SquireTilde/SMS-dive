using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundDetector : MonoBehaviour
{
    public event Action Land = delegate { };
    public event Action Unland = delegate { }; 


    void OnTriggerStay()
    {
        Land?.Invoke();
    }

    void OnTriggerExit()
    {
        Unland?.Invoke();
    }
}
