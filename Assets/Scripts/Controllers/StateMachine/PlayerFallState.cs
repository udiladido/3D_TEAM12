using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }
    public override void Enter()
    {
        stateMachine.IsFalling = true;
        base.Enter();
        StartAnimation(stateMachine.AnimationData.FallHash);
    }

    public override void Exit()
    {
        stateMachine.IsFalling = false;
        base.Exit();
        StopAnimation(stateMachine.AnimationData.FallHash);
        if (stateMachine.MovementType == Defines.CharacterMovementType.None)
            stateMachine.MoveDirection = Vector3.zero;
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.Player.ForceReceiver.IsGrounded())
        {
            if (stateMachine.MovementType == Defines.CharacterMovementType.None)
                stateMachine.ChangeState(stateMachine.IdleState);
            else
                stateMachine.ChangeState(stateMachine.RunState);
            return;
        }
    }
}