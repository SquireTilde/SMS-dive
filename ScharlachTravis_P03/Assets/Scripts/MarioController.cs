using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    private MarioInput _input = null;
    private MarioMotor _motor = null;
    private GroundDetector _grndDet = null;


    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpStrength = 100f;
    [SerializeField] private float _diveForce = 20f;

    private void Awake()
    {
        _input = GetComponent<MarioInput>();
        _motor = GetComponent<MarioMotor>();
        _grndDet = GetComponent<GroundDetector>();
    }

    private void OnEnable()
    {
        _input.MoveInput += OnMove;
        _input.JumpInput += OnJump;
        _input.DiveInput += OnDive;
        _grndDet.Land += OnLand;
        _grndDet.Unland += OnUnland;
    }

    private void OnDisable()
    {
        _input.MoveInput -= OnMove;
        _input.JumpInput -= OnJump;
        _input.DiveInput -= OnDive;
        _grndDet.Land -= OnLand;
        _grndDet.Unland -= OnUnland;
    }

    private void OnMove(Vector3 schmovement)
    {
        _motor.Move(schmovement * _moveSpeed);
 
    }

    private void OnJump()
    {
        _motor.Jump(_jumpStrength);
    }

    private void OnDive()
    {
        _motor.Dive(_diveForce);
    }

    private void OnLand()
    {
        _motor.Grounded();
    }

    private void OnUnland()
    {
        _motor.Airborne();
    }
}
