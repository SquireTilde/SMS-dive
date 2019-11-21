using System.Collections;
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
            Debug.Log("JUMP");
            JumpInput?.Invoke();
        }
    }

    void DetectDiveInput()
    {
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        if(Input.GetAxisRaw("Fire3") > 0)
        {
            DiveInput?.Invoke();
        }
   }
}
