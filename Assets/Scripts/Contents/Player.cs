using System;
using UnityEngine;

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

    [field: SerializeField]
    [field: Range(0, 10)]
    public float DodgeForce { get; private set; } = 3f;
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
        stateMachine.ChangeState(stateMachine.IdleState);
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

}