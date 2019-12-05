using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonkDetector : MonoBehaviour
{
    public event Action<Vector3, Vector3> Bonk = delegate { };
    //[SerializeField] private Transform _base = null;
    private Transform _tf = null;

    [SerializeField] float _rayDistance = 1f;

    void Awake()
    {
        _tf = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        RaycastHit info;
        Debug.DrawRay(_tf.position, _tf.forward * _rayDistance, Color.red, 1f);
        if (Physics.Raycast(_tf.position, _tf.forward, out info, _rayDistance))
        {
            Bonk?.Invoke(info.normal, info.point);
        }
    }
    
}
