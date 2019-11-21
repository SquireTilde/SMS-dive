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


        //Vector3 lookVector = (Vector3.forward + schmoveVector.normalized).normalized;
        Quaternion lookDirection = Quaternion.LookRotation(schmoveVector.normalized, Vector3.up);

        _rb.MovePosition(_rb.position + schmoveVector);
        _rb.MoveRotation(lookDirection);

        _frameSchmove = Vector3.zero;
    }


}
