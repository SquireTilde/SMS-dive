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
    public bool _isVaulting { private set; get; } = false;
    private bool _isJumping = false;
    private bool _justBonked = false;

    private Quaternion _lookDirection = Quaternion.identity;

    private float _physTimer = 0f;
    private float _getupTimer = 0f;
    [SerializeField] private float _getupDuration = 1f;

    [Header("Extra Hitboxes")]
    [SerializeField] GameObject _bonkDetect = null;

    [Header("Dive Stuff")]
    [SerializeField] float _sideDivePower = 7f;
    [SerializeField] float _upDivePower = 1f;
    [SerializeField] float _grndSideDivePower = 3f;
    [SerializeField] float _grndUpDivePower = 1f;
    [SerializeField] float _slideDrag = 0.3f;
    [SerializeField] float _normalDrag = 2f;

    [Header("Water Slide")]
    [SerializeField] float _waterSlidePower = 3f;

    [Header("Bonk Stuff")]
    [SerializeField] float _reflectPower = 1f;

    [Header("Misc Movement")]
    //[SerializeField] float _midairInfluence = .2f;
    [SerializeField] float _maxSpeed = 10f;
    [SerializeField] float _vaultForce = 200f;

    [Header("Particle Systems")]
    [SerializeField] ParticleSystem _slidePS = null;
    [SerializeField] ParticleSystem _bonkPS = null;
    [SerializeField] Transform _PStf = null;
    //[SerializeField] ParticleSystem _waterSlidePS = null;

    [Header("Audio")]
    [SerializeField] AudioSource _jumpAS = null;
    [SerializeField] AudioSource _diveAS = null;
    [SerializeField] AudioSource _slideAS = null;
    [SerializeField] AudioSource _vaultAS = null;
    [SerializeField] AudioSource _walkAS = null;
    [SerializeField] AudioSource _landAS = null;

    [Header("animation")]
    [SerializeField] Animator _anim = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _tf = GetComponent<Transform>();
        _bonkDetect.SetActive(false);
        Physics.autoSimulation = false;
        _walkAS.Pause();
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

        if (_isBonking)
            return;

        if(_bonkDetect.activeSelf)
            _bonkDetect.SetActive(false);
        _rb.AddForce(Vector3.up * jumpForce);

        _anim.Play("Jump");
        _isJumping = true;
        if (_isSliding)
        {
            _vaultAS.Play();
            _isVaulting = true;
            if (_rb.velocity.magnitude >= 2f)
            {
            _rb.AddForce(_tf.forward * _vaultForce);
            }

            return;
        }

        _jumpAS.Play();
    }

    public void Dive(float diveForce)
    {
        if (_isGrounded && !_isMoving)
            return;

        if (_isDiving)
            return;

        if (_isBonking)
            return;

        if (_isSliding)
        {
            _rb.AddForce(_lookDirection * (new Vector3(0f, _grndUpDivePower, _grndSideDivePower).normalized * diveForce * 1.3f));

            _isDiving = true;
            if(!_bonkDetect.activeSelf)
                _bonkDetect.SetActive(true);
            _diveAS.Play();
            return;
        }

        if (!_isGrounded)
        {
            _rb.AddForce(_lookDirection * (new Vector3(0f, _upDivePower, _sideDivePower).normalized * diveForce));
        }
        else
        {
            _walkAS.Pause();
            _rb.AddForce(_lookDirection * (new Vector3(0f, _grndUpDivePower, _grndSideDivePower).normalized * diveForce));
        }

        _isDiving = true;
        if (!_bonkDetect.activeSelf) ;
            _bonkDetect.SetActive(true);
        _diveAS.Play();
        _anim.Play("Dive");
    }

    public void Grounded(bool landing)
    {
        if (landing)
        {
            _landAS.Play();
            _isJumping = false;
        }


        if (_isDiving && landing)
            Slide();

        if (_isBonking && _justBonked)
        {
            Instantiate(_bonkPS, _PStf.position, Quaternion.identity);
            _justBonked = false;
            StartGetupTimer();
        }

        _isVaulting = false;
        _isGrounded = true;
    }

    public void Airborne()
    {
        _isGrounded = false;
        _isSliding = false;

        if (_walkAS.isPlaying)
        {
            _walkAS.Pause();
        }

        if (_slidePS.isPlaying)
        {
            _slidePS.Stop();
            _slidePS.Clear();
        }

        if (_slideAS.isPlaying)
        {
            _slideAS.Stop();
        }
    }

    public void Bonk(Vector3 normal, Vector3 point)
    {
        _isBonking = true;
        _justBonked = true;
        _isDiving = false;
        _isSliding = false;
        if(_bonkDetect.activeSelf)
            _bonkDetect.SetActive(false);
        if (_slidePS.isPlaying)
            _slidePS.Stop();
        if (_slideAS.isPlaying)
            _slideAS.Stop();

        Instantiate(_bonkPS, point, Quaternion.identity);

        Vector3 reflectedVelocity = Vector3.Reflect(_rb.velocity, normal);
        _rb.velocity = reflectedVelocity * _reflectPower;

        Vector3 lookVector = new Vector3(reflectedVelocity.x, 0, reflectedVelocity.z).normalized * -1;
        _lookDirection = Quaternion.LookRotation(lookVector, Vector3.up);
        _rb.MoveRotation(_lookDirection);
        _anim.Play("Bonked");
    }

    void Slide()
    {
        _isSliding = true;
        _isDiving = false;
        _slidePS.Play();
        _slideAS.Play();
        if(!_bonkDetect.activeSelf)
            _bonkDetect.SetActive(true);
    }

    public void WaterSlide()
    {
        _waterSliding = true;
        if (_slidePS.isPlaying)
            _slidePS.Stop();

        _rb.velocity = _rb.velocity * _waterSlidePower;
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


    void DoIGetUp()
    {
        if (_isSliding && (_getupTimer == 0f) && (_rb.velocity.magnitude <= 1f))
        {
            _slidePS.Stop();
            _slideAS.Stop();
            StartGetupTimer();
        }
    }

    void StartGetupTimer()
    {
        _getupTimer = _getupDuration;
    }

    void GetupTimer()
    {
        if (_getupTimer > 0f)
        {
            _getupTimer -= Time.deltaTime;
            if (_getupTimer == 0f)
            {
                StartGetup();
            }
        }
        if (_getupTimer < 0)
        {
            StartGetup();
        }
    }


    void StartGetup()
    {
        if(_bonkDetect.activeSelf) 
            _bonkDetect.SetActive(false);
        _getupTimer = 0;
        if (_isSliding)
            _anim.Play("Getup");

        _isSliding = false;
        _isBonking = false;
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
        DoIGetUp();

        ApplySchmovement(_frameSchmove);
        GetupTimer();
    }

    void ApplySchmovement(Vector3 schmoveVector)
    {
        if (schmoveVector == Vector3.zero)
        {
            _isMoving = false;
            _walkAS.Pause();
            return;
        }

        _isMoving = true;

        if (_isDiving)
            return;

        if (_isBonking)
            return;    



        if(_isSliding)
        {
            return;
        }

        if (_isGrounded && !_isSliding)
        { 
            _lookDirection = Quaternion.LookRotation(schmoveVector.normalized, Vector3.up);

            _rb.AddForce(schmoveVector);
            _rb.MoveRotation(_lookDirection);
            _walkAS.UnPause();
            if(!_isJumping && _rb.velocity.magnitude > 2f)
                _anim.Play("Run");
        }

        if (!_isGrounded)
        {
            _rb.AddForce(schmoveVector);
        }

        if (new Vector3(_rb.velocity.x, 0f, _rb.velocity.z).magnitude > _maxSpeed)
        {
            Vector3 clampedVelocity= Vector3.ClampMagnitude(new Vector3(_rb.velocity.x, 0f, _rb.velocity.z), _maxSpeed);
            _rb.velocity = clampedVelocity + new Vector3(0f, _rb.velocity.y, 0f);
        }

        _frameSchmove = Vector3.zero;
    }


}
