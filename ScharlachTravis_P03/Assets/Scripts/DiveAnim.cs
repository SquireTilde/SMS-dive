using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveAnim : MonoBehaviour
{
    [SerializeField] MarioMotor _motor = null;
    [SerializeField] Transform _base = null;
    private Transform _tf = null;
    [SerializeField] Animation _anim = null;

    [SerializeField] float _torque = 2f;

    void Awake()
    {
        _tf = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (_motor._isDiving)
        {
            _tf.rotation = Quaternion.Euler(_base.eulerAngles.x+110f, _base.eulerAngles.y, _base.eulerAngles.z);
        }
        else
        {
            _tf.rotation = _base.rotation;
        }
        
        /*while (_motor._isDiving)
        {
            _anim.Play();
        }

        if (!_motor._isDiving)
        {
            _anim.Stop();
        }*/
    }
    

}
