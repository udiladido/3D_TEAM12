public class PlayerRunState : PlayerGroundState
{
    private Defines.CharacterMovementType currentMovementType;
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.AnimationData.RunHash);
        stateMachine.Animator.SetFloat(stateMachine.AnimationData.MoveSpeedHash, GetAnimationMoveSpeed());
        currentMovementType = stateMachine.MovementType;
        switch (currentMovementType)
        {
            case Defines.CharacterMovementType.Backward:
                StartAnimation(stateMachine.AnimationData.BackwardHash);
                break;
            case Defines.CharacterMovementType.LeftStep:
                StartAnimation(stateMachine.AnimationData.LeftHash);
                break;
            case Defines.CharacterMovementType.RightStep:
                StartAnimation(stateMachine.AnimationData.RightHash);
                break;
            case Defines.CharacterMovementType.Forward:
                StartAnimation(stateMachine.AnimationData.ForwardHash);
                break;
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunHash);
        stateMachine.Animator.SetFloat(stateMachine.AnimationData.MoveSpeedHash, 1f);
        switch (currentMovementType)
        {
            case Defines.CharacterMovementType.Backward:
                StopAnimation(stateMachine.AnimationData.BackwardHash);
                break;
            case Defines.CharacterMovementType.LeftStep:
                StopAnimation(stateMachine.AnimationData.LeftHash);
                break;
            case Defines.CharacterMovementType.RightStep:
                StopAnimation(stateMachine.AnimationData.RightHash);
                break;
            case Defines.CharacterMovementType.Forward:
                StopAnimation(stateMachine.AnimationData.ForwardHash);
                break;
        }
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.MovementType == Defines.CharacterMovementType.None)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else
        {
            stateMachine.Animator.SetFloat(stateMachine.AnimationData.RunBlendHash, stateMachine.IsRunnung ? 1f : 0f);
        }
    }
}