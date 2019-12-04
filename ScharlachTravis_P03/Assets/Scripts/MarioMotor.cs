using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioMotor : MonoBehaviour
{
    Rigidbody _rb = null;
    Transform _tf = null;

    Vector3 _frameSchmove = Vector3.zero;
    public bool _isGrounded { private set; get; } = true;
    public bool _isMoving { private set; get; } = false;
    public bool _isDiving { private set; get; } = false;
    public bool _isSliding { private set; get; } = false;
    public bool _waterSliding { private set; get; } = false;
    public bool _isBonking { private set; get; } = false;

    private Quaternion _lookDirection = Quaternion.identity;

    private float _physTimer = 0f;

    [SerializeField] GameObject _bonkDetect = null;

    [SerializeField] float _sideDivePower = 7f;
    [SerializeField] float _upDivePower = 1f;
    [SerializeField] float _grndSideDivePower = 3f;
    [SerializeField] float _grndUpDivePower = 1f;

    [SerializeField] float _midairInfluence = .2f;
    [SerializeField] float _maxSpeed = 10f;

    [SerializeField] float _slideDrag = 0.3f;
    [SerializeField] float _normalDrag = 2f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _tf = GetComponent<Transform>();
        _bonkDetect.SetActive(false);
        Physics.autoSimulation = false;
    }

    public void Move(Vector3 schmovement)
    {
        _frameSchmove = schmovement;
    }

    public void Jump(float jumpForce)
    {
        if (!_isGrounded || _isDiving)
        {
            return;
        } 

        _rb.AddForce(Vector3.up * jumpForce);
    }

    public void Dive(float diveForce)
    {
        if (_isGrounded && !_isMoving)
            return;

        if (_isDiving)
            return;

        if (_isSliding)
        {
            //Dive rules are different if sliding
            return;
        }

        if (!_isGrounded)
        {
            _rb.AddForce(_lookDirection * (new Vector3(0f, _upDivePower, _sideDivePower).normalized * diveForce));
        }
        else
        {
            _rb.AddForce(_lookDirection * (new Vector3(0f, _grndUpDivePower, _grndSideDivePower).normalized * diveForce));
        }

        _isDiving = true;
        _bonkDetect.SetActive(true);
    }

    public void Grounded(bool landing)
    {
        if (_isDiving && landing)
            Slide();


        _isGrounded = true;
    }

    public void Airborne()
    {
        _isGrounded = false;
        _isSliding = false;
    }

    void Slide()
    {
        _isSliding = true;
        _isDiving = false;
    }

    void DiveDrag()
    {
        if(_isSliding || _isDiving)
        {
            _rb.drag = _slideDrag;
        }
        else
        {
            _rb.drag = _normalDrag;
        }
    }


    private void FixedUpdate()
    {
        _physTimer += Time.deltaTime;

        while (_physTimer >= Time.fixedDeltaTime)
        {
            _physTimer -= Time.fixedDeltaTime;
            Physics.Simulate(Time.fixedDeltaTime);
        }

        DiveDrag();


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
            
        if(!_isGrounded)
        {
            _rb.AddForce(schmoveVector * _midairInfluence);
            return;
        }

        if(_isSliding && !_waterSliding)
        {
            return;
        }
        else if(_isSliding && _waterSliding)
        {
            //i think using _midairInfluence only on Mario's X-axis will do the trick for the translation
            //rotation will require something different
            return;
        }

        if (_isGrounded)
        { 
            _lookDirection = Quaternion.LookRotation(schmoveVector.normalized, Vector3.up);

            _rb.AddForce(schmoveVector);
            _rb.MoveRotation(_lookDirection);
        }

        if(new Vector3(_rb.velocity.x, 0f, _rb.velocity.z).magnitude > _maxSpeed)
        {
            Vector3 clampedVelocity= Vector3.ClampMagnitude(new Vector3(_rb.velocity.x, 0f, _rb.velocity.z), _maxSpeed);
            _rb.velocity = clampedVelocity + new Vector3(0f, _rb.velocity.y, 0f);
        }

        _frameSchmove = Vector3.zero;
    }


}
