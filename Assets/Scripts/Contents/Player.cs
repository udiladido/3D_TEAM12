using System;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class Player : MonoBehaviour
{
    [field: SerializeField] public Condition Condition { get; private set; }
    [field: SerializeField] public AnimationData AnimationData { get; private set; }
    [field: SerializeField] public LayerMask LookLayerMask { get; private set; }
    public InputController Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }
    public Camera MainCamera { get; private set; }
    public ForceReceiveController ForceReceiver { get; private set; }

    private PlayerStateMachine stateMachine;

    [field: SerializeField]
    [field: Range(0, 1)]
    public float ForwardDotRange { get; private set; } = 0.5f;

    [field: SerializeField]
    [field: Range(0, 1)]
    public float RightDotRange { get; private set; } = 0.5f;

    [field: Header("이동 방향에 따른 속도 비율")]
    [field: SerializeField]
    [field: Range(0, 1)]
    public float BackwardSpeedRatio { get; private set; } = 0.5f;

    [field: SerializeField]
    [field: Range(0, 1)]
    public float SideSpeedRatio { get; private set; } = 0.75f;

    [field: Header("회피 및 점프 힘")]
    [field: SerializeField]
    [field: Range(0, 50)]
    public float DodgeForce { get; private set; } = 10f;

    [field: SerializeField]
    [field: Range(0, 50)]
    public float JumpForce { get; private set; } = 10f;

    [field: SerializeField] public bool FixedCameraFacing { get; private set; }

    private void Awake()
    {
        Input = GetComponent<InputController>();
        Animator = GetComponentInChildren<Animator>();
        Condition = GetComponent<Condition>();
        Controller = GetComponent<CharacterController>();
        MainCamera = Camera.main;
        ForceReceiver = GetComponent<ForceReceiveController>();

        stateMachine = new PlayerStateMachine(this);
        AnimationData.Initialize();
    }

    private void Start()
    {
        Revive();
        Condition.OnHit += Hit;
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public void Die()
    {
        Condition.OnDead -= Die;
        stateMachine.ChangeState(stateMachine.DeadState);
    }

    public void Revive()
    {
        Condition.OnDead += Die;
        Condition.FullRecovery();
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    public void Hit()
    {
        stateMachine.Play(stateMachine.HitState);
    }

    public void Jump()
    {
        ForceReceiver?.Reset();
        ForceReceiver?.Jump(JumpForce);
    }

    public void FixCameraFacing()
    {
        Camera mainCamera = Camera.main;
        if (FixedCameraFacing)
        {
            mainCamera.transform.SetParent(null);
            mainCamera.transform.position = transform.position + new Vector3(-4, 21, 10);
            mainCamera.transform.rotation = UnityEngine.Quaternion.Euler(62, 168, 0);
            
            FixedCameraFacing = false;
        }
        else
        {
            mainCamera.transform.SetParent(transform);
            mainCamera.transform.localPosition = new Vector3(0, 1, 6);
            mainCamera.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180, 0);

            FixedCameraFacing = true;
        }
    }
}