using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindToObject : MonoBehaviour
{
    [SerializeField] Transform _targetObject = null;
    private Transform _thisTF = null;


    [Header("Constraints")]
    [SerializeField] bool _xPos = false;
    [SerializeField] bool _yPos = false;
    [SerializeField] bool _zPos = false;

    [SerializeField] bool _xRot = false;
    [SerializeField] bool _yRot = false;
    [SerializeField] bool _zRot = false;

    float _xPosValue = 0;
    float _yPosValue = 0;
    float _zPosValue = 0;

    float _xRotValue = 0;
    float _yRotValue = 0;
    float _zRotValue = 0;


    void Awake()
    {
        _thisTF = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        UpdateValues();

        _thisTF.position = new Vector3(_xPosValue,_yPosValue,_zPosValue);
        _thisTF.rotation = Quaternion.Euler(_xRotValue, _yRotValue, _zRotValue);
    }

    void UpdateValues()
    {
        //Position
        if (_xPos)
            _xPosValue = _thisTF.position.x;
        else
            _xPosValue = _targetObject.position.x;

        if (_yPos)
            _yPosValue = _thisTF.position.y;
        else
            _yPosValue = _targetObject.position.y;

        if (_zPos)
            _zPosValue = _thisTF.position.z;
        else
            _zPosValue = _targetObject.position.z;

        //Rotation
        if (_xRot)
            _xRotValue = _thisTF.eulerAngles.x;
        else
            _xRotValue = _targetObject.eulerAngles.x;

        if (_yRot)
            _yRotValue = _thisTF.eulerAngles.y;
        else
            _yRotValue = _targetObject.eulerAngles.y;

        if (_zRot)
            _zRotValue = _thisTF.eulerAngles.z;
        else
            _zRotValue = _targetObject.eulerAngles.z;
    }
}