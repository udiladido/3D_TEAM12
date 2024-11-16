using UnityEngine;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public virtual void Enter()
    {
        Debug.Log($"{GetType().Name} Enter");
        InputController input = stateMachine.Input;
        if (input == null) return;

        input.OnMoveEvent += MoveHandle;
        input.OnMoveCancelEvent += MoveCancelHandle;
        input.OnLookEvent += LookHandle;
        input.OnDodgeEvent += DodgeHandle;
        input.OnJumpEvent += JumpHandle;
        input.OnRunEvent += RunHandle;
        input.OnAttackEvent += AttackHandle;
        input.OnAttackCancelEvent += AttackCancelHandle;
    }
    public virtual void Exit()
    {
        InputController input = stateMachine.Input;
        if (input == null) return;

        input.OnMoveEvent -= MoveHandle;
        input.OnMoveCancelEvent -= MoveCancelHandle;
        input.OnLookEvent -= LookHandle;
        input.OnDodgeEvent -= DodgeHandle;
        input.OnJumpEvent -= JumpHandle;
        input.OnRunEvent -= RunHandle;
        input.OnAttackEvent -= AttackHandle;
        input.OnAttackCancelEvent -= AttackCancelHandle;
    }
    protected void AttackHandle(Defines.CharacterAttackInputType attackInputType)
    {
        stateMachine.IsAttacking = true;
    }
    protected void AttackCancelHandle(Defines.CharacterAttackInputType attackInputType)
    {
        stateMachine.IsAttacking = false;
        // TODO : 키를 계속 누르고 있다가 떼면 할 일들..
    }
    protected void MoveHandle(Vector2 inputValue)
    {
        stateMachine.MoveDirection = GetMoveDirection(new Vector3(inputValue.x, 0, inputValue.y));
        stateMachine.MovementType = GetMovementType(stateMachine.MoveDirection);
        if (stateMachine.Player.ForceReceiver.IsGrounded())
            MoveChange();

    }
    protected void MoveCancelHandle()
    {
        stateMachine.MoveDirection = Vector3.zero;
        stateMachine.MovementType = Defines.CharacterMovementType.None;
    }
    protected void LookHandle(Vector2 mousePosition)
    {
        Ray ray = stateMachine.Player.MainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, stateMachine.Player.LookLayerMask))
        {
            Vector3 targetPosition = hit.point;
            Vector3 lookDirection = (targetPosition - stateMachine.Player.transform.position);
            lookDirection.y = 0;
            lookDirection.Normalize();

            stateMachine.LookDirection = lookDirection;
        }
    }
    protected void DodgeHandle()
    {
        if (stateMachine.MovementType == Defines.CharacterMovementType.None) return;
        if (stateMachine.IsRunnung == false) return;
        stateMachine.ChangeState(stateMachine.DodgeState);
    }
    protected void JumpHandle()
    {
        if (stateMachine.JumpCount >= 2) return;
        stateMachine.ChangeState(stateMachine.JumpState);
    }
    private void RunHandle(bool isRunning)
    {
        if (stateMachine.IsRunnung == isRunning) return;
        stateMachine.IsRunnung = isRunning;
        if (stateMachine.MovementType == Defines.CharacterMovementType.None) return;
        if (isRunning)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
        else
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }
    protected void MoveChange()
    {
        if (stateMachine.IsRunnung)
            stateMachine.ChangeState(stateMachine.RunState);
        else
            stateMachine.ChangeState(stateMachine.WalkState);
    }

    public void StartAnimation(int animationHash)
    {
        stateMachine.Animator.SetBool(animationHash, true);
    }
    public void StopAnimation(int animationHash)
    {
        stateMachine.Animator.SetBool(animationHash, false);
    }
    private void ApplyMove(Vector3 direction)
    {
        if (stateMachine.MovementType != Defines.CharacterMovementType.None)
        {
            float moveSpeed = GetMoveSpeed(stateMachine.MovementType);
            Vector3 movement = (direction * moveSpeed) + stateMachine.Player.ForceReceiver.Movement;
            stateMachine.Controller.Move(movement * Time.deltaTime);
        }
        else
        {
            // 가만히 있어도 낙하는 계속 해야한다.
            stateMachine.Controller.Move(stateMachine.Player.ForceReceiver.Movement * Time.deltaTime);
        }
    }
    private void Move()
    {
        ApplyMove(stateMachine.MoveDirection);
    }
    private float GetMoveSpeed(Defines.CharacterMovementType movementType)
    {
        float maxMoveSpeed = stateMachine.Condition.CurrentStat.moveSpeed;

        float sideSpeedRatio = stateMachine.Player.SideSpeedRatio;
        float backwardSpeedRatio = stateMachine.Player.BackwardSpeedRatio;

        if (stateMachine.IsRunnung == false)
            return maxMoveSpeed * backwardSpeedRatio;

        switch (movementType)
        {
            case Defines.CharacterMovementType.Backward:
                maxMoveSpeed *= backwardSpeedRatio;
                break;
            case Defines.CharacterMovementType.LeftStep:
                maxMoveSpeed *= sideSpeedRatio;
                break;
            case Defines.CharacterMovementType.RightStep:
                maxMoveSpeed *= sideSpeedRatio;
                break;
            case Defines.CharacterMovementType.Forward:
                break;
        }

        return maxMoveSpeed;
    }
    private Vector3 GetMoveDirection(Vector3 inputDirection)
    {
        // 카메라 보는 방향을 앞이라고 보고 이동 방향을 정한다.
        Vector3 cameraForward = stateMachine.Player.MainCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 cameraRight = stateMachine.Player.MainCamera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();
        return (cameraForward * inputDirection.z + cameraRight * inputDirection.x).normalized;
    }

    private void Rotate()
    {
        if (stateMachine.LookDirection == Vector3.zero) return;
        Transform playerTransform = stateMachine.Player.transform;
        playerTransform.forward = stateMachine.LookDirection;
    }
    /// <summary>
    /// 이동 방향에 따른 캐릭터의 움직임을 정한다.
    /// </summary>
    /// <returns></returns>
    private Defines.CharacterMovementType GetMovementType(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return Defines.CharacterMovementType.None;

        Transform playerTransform = stateMachine.Player.transform;
        float forwardDotRange = stateMachine.Player.ForwardDotRange;
        float rightDotRange = stateMachine.Player.RightDotRange;
        float forwardDot = Vector3.Dot(playerTransform.forward, direction);
        float rightDot = Vector3.Dot(playerTransform.right, direction);

        // 화면에서 볼때 캐릭터가 아래로 보면 z축이 -1, 위로 보면 z축이 1, 왼쪽으로 보면 x축이 -1, 오른쪽으로 보면 x축이 1 이다.
        // 이동 방향이 위쪽(즉 z 축이 1)이고 캐릭터가 바라보는 방향아 아래쪽일 때, 캐릭터는 뒷걸음질 치는 모션을 취해야하고, 캐릭터가 바라보는 방향으로 이동 할때는 전진하는 모션을 취해야하고, 바라보는 방향에서 좌, 우로 움직일때는 사이드 스텝으로 모션을 취해야한다.
        // 양 방향간의 내적으로 캐릭터의 움직임을 정해 애니메이션을 취하도록 한다.

        if (forwardDot < -forwardDotRange)
        {
            return Defines.CharacterMovementType.Backward;
        }
        else if (rightDot > rightDotRange)
        {
            return Defines.CharacterMovementType.RightStep;
        }
        else if (rightDot < -rightDotRange)
        {
            return Defines.CharacterMovementType.LeftStep;
        }
        else
        {
            return Defines.CharacterMovementType.Forward;
        }
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        //0.0: 애니메이션이 처음 시작된 상태입니다.
        // 1.0: 애니메이션이 한 번 완료된 상태입니다.
        // 1.0: 애니메이션이 반복 재생될 경우, 1을 넘어 계속 증가합니다.
        // 예를 들어, normalizedTime이 2.5라면
        // 애니메이션이 두 번 재생된 후 세 번째 재생의 50%가 진행된 것을 의미합니다. 
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    public virtual void HandleInput()
    {

    }
    public virtual void Update()
    {
        if (stateMachine.Player.FixedCameraFacing == false)
            Rotate();

        Move();
        Attack();
    }
    private void Attack()
    {
        if (stateMachine.IsAttacking)
            stateMachine.Play(stateMachine.AttackState);
    }
    public virtual void PhysicsUpdate()
    {

    }
}