using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }
    public override void Enter()
    {
        stateMachine.Input.OnJumpEvent += JumpHandle;
        stateMachine.Input.OnMoveEvent += MoveHandle;
        stateMachine.Input.OnMoveCancelEvent += MoveCancelHandle;
        stateMachine.Input.OnLookEvent += LookHandle;
        
        StartAnimation(stateMachine.AnimationData.AirHash);
    }

    public override void Exit()
    {
        stateMachine.Input.OnJumpEvent -= JumpHandle;
        stateMachine.Input.OnMoveEvent -= MoveHandle;
        stateMachine.Input.OnMoveCancelEvent -= MoveCancelHandle;
        stateMachine.Input.OnLookEvent -= LookHandle;
        
        StopAnimation(stateMachine.AnimationData.AirHash);
    }
}