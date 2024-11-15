using System;
using UnityEngine;

/// <summary>
/// 힘을 받는 컨트롤러
/// </summary>
public class ForceReceiveController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float drag = 0.3f;
    
    private Vector3 dampingVeolcity;
    private Vector3 impact;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVeolcity, drag);
    }

    public void Reset()
    {
        impact = Vector3.zero;
    }
    
    public void AddForce(Vector3 force)
    {
        impact += force;
    }

    public bool IsImpaceZero()
    {
        return impact.sqrMagnitude < 0.1f;
    }

    public Vector3 GetForceMovement(Vector3 direction)
    {
        return impact + direction;
    }
}