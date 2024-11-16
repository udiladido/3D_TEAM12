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

    [field: Header("땅 인식")][field: SerializeField]
    private Vector3 groundCheckOffset = Vector3.zero;
    [field: SerializeField] private Vector3 groundCheckSize = new Vector3(1f, 0.4f, 1f);
    [field: SerializeField] private LayerMask groundLayerMask;
    [field: SerializeField] private Transform groundCheck;

    [field: Header("경사면 인식")][field: SerializeField]
    private float slopeRayDistance = 1.5f;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        bool isOnSlope = IsOnSlope();
        // bool isGrounded = IsGrounded();

        Debug.Log($"isGrounded: {controller.isGrounded}, isOnSlope: {isOnSlope}");

        if (verticalVelocity < 0f && controller.isGrounded && isOnSlope)
        {
            verticalVelocity += Physics.gravity.y * controller.velocity.magnitude * Time.deltaTime;
        }
        else if (verticalVelocity < 0f && controller.isGrounded)
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

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }


    public bool IsGrounded()
    {
        if (groundCheck == null)
            groundCheck = FindOrCreateGroundCheck();

        groundCheck.position = transform.position + groundCheckOffset;
        groundCheck.localScale = groundCheckSize;

        return Physics.CheckBox(groundCheck.position, groundCheckSize * 0.5f, Quaternion.identity, groundLayerMask);
    }

    public bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, slopeRayDistance, groundLayerMask))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
            return slopeAngle != 0f && slopeAngle <= controller.slopeLimit;
        }

        return false;
    }

    private void OnDrawGizmos()
    {


        Gizmos.color = IsGrounded() ? Color.blue : Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCheck.localScale);
        Gizmos.color = IsOnSlope() ? Color.red : Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down * slopeRayDistance);
    }

    private Transform FindOrCreateGroundCheck()
    {
        Transform groundCheck = Utils.FindChild(gameObject, "GroundCheck")?.transform;
        if (groundCheck == null)
        {
            groundCheck = new GameObject("GroundCheck").transform;
            groundCheck.SetParent(transform);
        }
        return groundCheck;
    }
}