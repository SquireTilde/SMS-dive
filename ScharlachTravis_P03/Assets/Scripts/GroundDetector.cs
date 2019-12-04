using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundDetector : MonoBehaviour
{
    public event Action<bool> Land = delegate { };
    public event Action Unland = delegate { };
    bool _Landing = false;


    void OnTriggerStay()
    {
        Land?.Invoke(_Landing);
        _Landing = false;
    }

    void OnTriggerExit()
    {
        Unland?.Invoke();
        _Landing = true;
    }
}
