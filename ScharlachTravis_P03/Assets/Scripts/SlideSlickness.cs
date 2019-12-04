using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSlickness : MonoBehaviour
{
    private CapsuleCollider _cc = null;
    private MarioMotor _motor = null;

    [SerializeField] private PhysicMaterial _normal = null;
    [SerializeField] private PhysicMaterial _slide = null;

    // Start is called before the first frame update
    void Awake()
    {
        _cc = GetComponent<CapsuleCollider>();
        _motor = GetComponent<MarioMotor>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_motor._isSliding)
        {
            _cc.material = _slide;
        }
        else
            _cc.material = _normal;
    }
}
