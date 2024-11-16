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
    
    private float verticalVelocity;
    
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVeolcity, drag);
    }

    public void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }
    
    public void AddForce(Vector3 force)
    {
        impact += force;
    }
}