using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        stateMachine.JumpCount += 1;
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        if (currentInfo.IsTag("Jump"))
            stateMachine.Animator.Play(currentInfo.shortNameHash, -1, 0f);
        else
            StartAnimation(stateMachine.AnimationData.JumpHash);

        stateMachine.Player.Jump();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.AnimationData.JumpHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (stateMachine.Player.Controller.velocity.y <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}