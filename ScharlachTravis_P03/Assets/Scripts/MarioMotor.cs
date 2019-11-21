using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioMotor : MonoBehaviour
{
    Rigidbody _rb = null;

    Vector3 _frameSchmove = Vector3.zero;
    public bool _isGrounded { private set; get; } = true;
    public bool _isMoving { private set; get; } = false;
    public bool _isDiving { private set; get; } = false;

    private Quaternion _lookDirection = Quaternion.identity;

    [SerializeField] float _sideDivePower = 7f;
    [SerializeField] float _upDivePower = 1f;
    [SerializeField] float _grndSideDivePower = 3f;
    [SerializeField] float _grndUpDivePower = 1f;

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
        if (!_isGrounded || _isDiving)
            return;

        _rb.AddForce(Vector3.up * jumpForce);
    }

    public void Dive(float diveForce)
    {
        if (_isGrounded && !_isMoving)
            return;

        if (_isDiving)
            return;

        if (!_isGrounded)
        {
            _rb.AddForce(_lookDirection * (new Vector3(0f, _upDivePower, _sideDivePower).normalized * diveForce));
        }
        else
        {
            _rb.AddForce(_lookDirection * (new Vector3(0f, _grndUpDivePower, _grndSideDivePower).normalized * diveForce));
        }

        _isDiving = true;
    }

    public void Grounded()
    {
        _isGrounded = true;

        //This should go somewhere else, but it's here until i implement the diving hitboxes
        if (_isDiving)
            _isDiving = false;
    }

    public void Airborne()
    {
        _isGrounded = false;
    }


    private void FixedUpdate()
    {
        ApplySchmovement(_frameSchmove);
    }

    void ApplySchmovement(Vector3 schmoveVector)
    {
        if (schmoveVector == Vector3.zero)
        {
            _isMoving = false;
            return;
        }

        _isMoving = true;

        if (_isDiving)
            return;
            
        _lookDirection = Quaternion.LookRotation(schmoveVector.normalized, Vector3.up);

        _rb.MovePosition(_rb.position + schmoveVector);
        _rb.MoveRotation(_lookDirection);

        _frameSchmove = Vector3.zero;
    }


}
