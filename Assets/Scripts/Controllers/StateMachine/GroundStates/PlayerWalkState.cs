public class PlayerWalkState : PlayerGroundState
{
    private Defines.CharacterMovementType currentMovementType;
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.AnimationData.WalkHash);
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
        StopAnimation(stateMachine.Player.AnimationData.WalkHash);
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
            return;
        }
    }
}