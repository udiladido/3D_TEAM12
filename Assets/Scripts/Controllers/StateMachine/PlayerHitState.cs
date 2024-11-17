public class PlayerHitState : PlayerBaseState
{
    public PlayerHitState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        MoveCancelHandle();
        if (stateMachine.Player.ForceReceiver.IsGrounded())
            StartAnimation(stateMachine.AnimationData.GroundHash);
        else
            StartAnimation(stateMachine.AnimationData.AirHash);

        StartAnimation(stateMachine.AnimationData.HitHash);
    }

    public override void Exit()
    {
        if (stateMachine.Player.ForceReceiver.IsGrounded())
            StartAnimation(stateMachine.AnimationData.GroundHash);
        else
            StartAnimation(stateMachine.AnimationData.AirHash);

        StopAnimation(stateMachine.AnimationData.HitHash);
    }

    public override void Update()
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Hit");
        if (normalizedTime >= 1f)
            stateMachine.ChangeState(stateMachine.IdleState);
    }
}