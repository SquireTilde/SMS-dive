using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioMotor : MonoBehaviour
{
    Rigidbody _rb = null;

    Vector3 _frameSchmove = Vector3.zero;
    bool _isGrounded = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 schmovement)
    {
        _frameSchmove = schmovement;
    }

    public void Jump(float jumpForce)
    {
        if (_isGrounded == false)
            return;

        _rb.AddForce(Vector3.up * jumpForce);
    }


    private void FixedUpdate()
    {
        ApplySchmovement(_frameSchmove);
    }

    void ApplySchmovement(Vector3 schmoveVector)
    {
        if (schmoveVector == Vector3.zero)
            return;

        _rb.MovePosition(_rb.position + schmoveVector);


        _frameSchmove = Vector3.zero;
    }


}
