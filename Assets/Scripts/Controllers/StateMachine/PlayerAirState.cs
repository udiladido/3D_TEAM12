using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }
    public override void Enter()
    {
        InputController input = stateMachine.Input;
        if (input == null) return;
        
        input.OnMoveEvent += MoveHandle;
        input.OnMoveCancelEvent += MoveCancelHandle;
        
        StartAnimation(stateMachine.AnimationData.AirHash);
    }

    public override void Exit()
    {
        InputController input = stateMachine.Input;
        if (input == null) return;
        
        input.OnMoveEvent -= MoveHandle;
        input.OnMoveCancelEvent -= MoveCancelHandle;
        
        StopAnimation(stateMachine.AnimationData.AirHash);
    }
}