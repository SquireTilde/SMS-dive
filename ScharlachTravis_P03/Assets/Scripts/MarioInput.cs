using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MarioInput : MonoBehaviour
{
    public event Action<Vector3> MoveInput = delegate { };
    public event Action JumpInput = delegate { };

    [SerializeField] Transform _camera = null;


    private void Update()
    {
        DetectMoveInput();
        DetectJumpInput();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpInput?.Invoke();
        }
    }
}
