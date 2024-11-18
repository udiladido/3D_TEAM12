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
    public Combat Combat { get; private set; }
    public Equipment Equipment { get; private set; }

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
    private bool canControl = true;
    private void Awake()
    {
        MainCamera = Camera.main;
        Input = GetComponent<InputController>();
        Animator = GetComponentInChildren<Animator>();
        Condition = GetComponent<Condition>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiveController>();
        Combat = GetComponent<Combat>();
        Equipment = GetComponent<Equipment>();

        stateMachine = new PlayerStateMachine(this);
        AnimationData.Initialize();
    }

    private void Update()
    {
        if (canControl == false) return;
        
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    public void SetJob(JobEntity job)
    {
        if (Animator != null) Animator = null;
        GameObject jobGo = Managers.Resource.Instantiate(job.prefabPath, transform);
        jobGo.transform.localPosition = Vector3.zero;
        if (Equipment == null) Equipment = GetComponent<Equipment>();
        Animator = GetComponentInChildren<Animator>();
        Equipment.LoadModel();
        Condition.SetData(job.jobStatEntity);
        Revive();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public void Die()
    {
        Condition.OnDead -= Die;
        Condition.OnHit -= Hit;
        stateMachine.ChangeState(stateMachine.DeadState);
        canControl = false;
    }

    public void Revive()
    {
        Condition.OnDead += Die;
        Condition.OnHit += Hit;
        Condition.FullRecovery();
        stateMachine.ChangeState(stateMachine.IdleState);
        canControl = true;
    }

    public void Hit()
    {
        stateMachine.Hit();
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