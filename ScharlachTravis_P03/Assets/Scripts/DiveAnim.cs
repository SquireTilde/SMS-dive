using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveAnim : MonoBehaviour
{
    [SerializeField] MarioMotor _motor = null;
    [SerializeField] Transform _base = null;
    private Transform _tf = null;
    //[SerializeField] Animation _anim = null;
    [SerializeField] Transform _origin = null;

   // [SerializeField] float _torque = 2f;

    void Awake()
    {
        _tf = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (_motor._isDiving)
        {
            Quaternion from = _tf.rotation;
            Quaternion to = Quaternion.Euler(_base.eulerAngles.x + 110f, _base.eulerAngles.y, _base.eulerAngles.z);

            _tf.rotation = Quaternion.Slerp(from, to, Time.deltaTime * 3);
        }
        else if (_motor._isSliding)
        {
            RaycastHit raycastInfo;
            if (Physics.Raycast(_origin.position, Vector3.down, out raycastInfo, Mathf.Infinity))
            {
               Vector3 normalFloor = raycastInfo.normal;
               Vector3 invertFloor = new Vector3(normalFloor.x * -1, normalFloor.y * -1, normalFloor.z * -1);
               Quaternion lookFloor = Quaternion.LookRotation(invertFloor);
               _tf.rotation = Quaternion.Euler(lookFloor.eulerAngles.x, _base.eulerAngles.y, _base.eulerAngles.z);
            }
            //_tf.rotation = Quaternion.Euler(_base.eulerAngles.x + 90f, _base.eulerAngles.y, _base.eulerAngles.z);
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
