using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }
    public override void Enter()
    {
        // 죽으면 아무런 행위를 할 수 없기 때문에 이벤트 사용 못하도록 한다.
        // base.Enter();
        StartAnimation(stateMachine.AnimationData.DeadHash);
        MoveCancelHandle();
    }

    public override void Exit()
    {
        // base.Exit();
        StopAnimation(stateMachine.AnimationData.DeadHash);
    }
}