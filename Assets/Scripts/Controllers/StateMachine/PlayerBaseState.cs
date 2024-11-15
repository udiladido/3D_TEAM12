using UnityEngine;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;

    public virtual void Enter()
    {
        InputController input = stateMachine.Input;
        if (input == null) return;

        input.OnMoveEvent += MoveHandle;
        input.OnLookEvent += LookHandle;
        input.OnDodgeEvent += DodgeHandle;
    }
    public virtual void Exit()
    {
        InputController input = stateMachine.Input;
        if (input == null) return;

        input.OnMoveEvent -= MoveHandle;
        input.OnLookEvent -= LookHandle;
        input.OnDodgeEvent -= DodgeHandle;
    }
    private void MoveHandle(Vector2 inputValue)
    {
        if (inputValue == Vector2.zero)
        {
            stateMachine.MoveDirection = Vector3.zero;
        }
        else
        {
            stateMachine.MoveDirection = GetMoveDirection(new Vector3(inputValue.x, 0, inputValue.y));
        }
    }
    private void LookHandle(Vector2 mousePosition)
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
    private void DodgeHandle()
    {
        if (stateMachine.MovementType == Defines.CharacterMovementType.None) return;
        
        switch (stateMachine.MovementType)
        {
            case Defines.CharacterMovementType.Forward:
                stateMachine.ChangeState(stateMachine.DodgeForwardState);
                break;
            case Defines.CharacterMovementType.Backward:
                stateMachine.ChangeState(stateMachine.DodgeBackwardState);
                break;
            case Defines.CharacterMovementType.LeftStep:
                stateMachine.ChangeState(stateMachine.DodgeLeftStepState);
                break;
            case Defines.CharacterMovementType.RightStep:
                stateMachine.ChangeState(stateMachine.DodgeRightStepState);
                break;
        }
    }
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void StartAnimation(int animationHash)
    {
        stateMachine.Animator.SetBool(animationHash, true);
    }

    public void StopAnimation(int animationHash)
    {
        stateMachine.Animator.SetBool(animationHash, false);
    }

    private void Move()
    {
        if (stateMachine.MoveDirection == Vector3.zero)
        {
            ChangeRunState(Defines.CharacterMovementType.None);
            return;
        }

        Defines.CharacterMovementType currentMovementType = GetMovementType(stateMachine.MoveDirection);
        float moveSpeed = GetMoveSpeed(currentMovementType);
        Vector3 forceMovement = stateMachine.Player.ForceReceiver.GetForceMovement(stateMachine.MoveDirection);
        Vector3 movement = (stateMachine.MoveDirection * moveSpeed) + forceMovement;
        stateMachine.Controller.Move(movement * Time.deltaTime);
        ChangeRunState(currentMovementType);
    }
    private float GetMoveSpeed(Defines.CharacterMovementType movementType)
    {
        float maxMoveSpeed = stateMachine.Condition.CurrentStat.moveSpeed;

        float sideSpeedRatio = stateMachine.Player.SideSpeedRatio;
        float backwardSpeedRatio = stateMachine.Player.BackwardSpeedRatio;

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
    protected void ChangeRunState(Defines.CharacterMovementType movementType)
    {
        if (stateMachine.MovementType == movementType) return;

        switch (movementType)
        {
            case Defines.CharacterMovementType.None:
                stateMachine.ChangeState(stateMachine.IdleState);
                break;
            case Defines.CharacterMovementType.Backward:
                stateMachine.ChangeState(stateMachine.RunBackwordState);
                break;
            case Defines.CharacterMovementType.LeftStep:
                stateMachine.ChangeState(stateMachine.RunLeftStepState);
                break;
            case Defines.CharacterMovementType.RightStep:
                stateMachine.ChangeState(stateMachine.RunRightStepState);
                break;
            case Defines.CharacterMovementType.Forward:
                stateMachine.ChangeState(stateMachine.RunForwardState);
                break;
        }

        stateMachine.MovementType = movementType;
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

    public virtual void HandleInput()
    {

    }
    public virtual void Update()
    {
        Rotate();
        Move();
    }
    public virtual void PhysicsUpdate()
    {

    }
}