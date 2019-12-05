using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPuddle : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        MarioMotor motor = other.GetComponent<MarioMotor>();
        if(motor != null)
        {
            if(motor._isDiving)
            {
                motor.WaterSlide();
            }
        }
    }
}
