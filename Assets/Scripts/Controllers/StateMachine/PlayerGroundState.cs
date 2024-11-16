using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.AnimationData.GroundHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.AnimationData.GroundHash);
    }
    
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (
            stateMachine.Player.ForceReceiver.IsGrounded() == false
            && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime
            )
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}