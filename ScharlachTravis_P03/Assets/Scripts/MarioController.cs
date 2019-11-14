using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    private MarioInput _input = null;
    private MarioMotor _motor = null;


    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpStrength = 100f;

    private void Awake()
    {
        _input = GetComponent<MarioInput>();
        _motor = GetComponent<MarioMotor>();
    }

    private void OnEnable()
    {
        _input.MoveInput += OnMove;
        _input.JumpInput += OnJump;
    }

    private void OnDisable()
    {
        _input.MoveInput -= OnMove;
        _input.JumpInput -= OnJump;
    }

    private void OnMove(Vector3 schmovement)
    {
        _motor.Move(schmovement * _moveSpeed);
 
    }

    private void OnJump()
    {
        _motor.Jump(_jumpStrength);
    }
}
