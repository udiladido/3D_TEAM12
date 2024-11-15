using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    private bool alreadyAppliedForce;
    private float dodgeTime;
    public PlayerDodgeState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        alreadyAppliedForce = false;
        dodgeTime = 0.5f;
        StartAnimation(stateMachine.Player.AnimationData.DodgeHash);
        // TODO : 재사용 대기시간 설정
        TryApplyForce();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.DodgeHash);
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) return;
        alreadyAppliedForce = true;

        stateMachine.Player.ForceReceiver.Reset();
        stateMachine.Player.ForceReceiver.AddForce(stateMachine.MoveDirection * stateMachine.Player.DodgeForce);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Player.ForceReceiver.IsImpaceZero())
            ChangeRunState(stateMachine.MovementType);
    }
}