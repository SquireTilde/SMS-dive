using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillInXSec : MonoBehaviour
{
    [SerializeField] float _timeUntilDeath = 1.0f;
    private void Awake()
    {
        Destroy(gameObject, _timeUntilDeath);
    }
}
