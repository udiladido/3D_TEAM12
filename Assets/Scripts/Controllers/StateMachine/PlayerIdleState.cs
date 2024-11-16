using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.AnimationData.IdleHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.AnimationData.IdleHash);
    }
    
}