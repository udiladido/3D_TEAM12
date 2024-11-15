public class PlayerDodgeLeftStepState : PlayerDodgeState
{
    public PlayerDodgeLeftStepState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        
    }
    
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.LeftHash);
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.LeftHash);
    }
}