﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MarioInput : MonoBehaviour
{
    public event Action<Vector3> MoveInput = delegate { };
    public event Action JumpInput = delegate { };
    public event Action DiveInput = delegate { };

    [SerializeField] Transform _camera = null;

    private MarioMotor _motor = null;

    bool _jmp = false;
    bool _dve = false;

    private void Awake()
    {
        _motor = GetComponent<MarioMotor>();
    }


    private void Update()
    {
        DetectMoveInput();
        DetectJumpInput();
        DetectDiveInput();
    }


    void DetectMoveInput()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        if(xInput != 0 || yInput != 0)
        {
            Vector3 _sidewaysMovement = _camera.right * xInput;
            Vector3 _forwardMovement = _camera.forward * yInput;
            Vector3 movement = (_sidewaysMovement + _forwardMovement).normalized;

            MoveInput?.Invoke(movement);
        }
    }

    void DetectJumpInput()
    {
        if (Input.GetAxisRaw("Jump") > 0)
        {
            if (!_jmp)
            {
                _jmp = true;
                JumpInput?.Invoke();
            }
        }
        if (Input.GetAxisRaw("Jump") == 0)
        {
            _jmp = false;
        }
    }

    void DetectDiveInput()
    {
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        if (Input.GetAxisRaw("Fire3") > 0)
        {
            if (!_dve)
            {
                _dve = true;
                DiveInput?.Invoke();
            }
        }

        if (Input.GetAxisRaw("Fire3") == 0)
        {
            _dve = false;
        }
    }
}
