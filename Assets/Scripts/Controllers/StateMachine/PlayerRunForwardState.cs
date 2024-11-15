public class PlayerRunForwardState : PlayerRunState
{
    public PlayerRunForwardState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        
    }
    
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.ForwardHash);
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.ForwardHash);
    }
}