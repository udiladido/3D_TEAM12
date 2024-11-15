public class PlayerRunRightStepState : PlayerRunState
{
    public PlayerRunRightStepState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        
    }
    
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.RightHash);
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RightHash);
    }
}