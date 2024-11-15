public class PlayerDodgeBackwardState : PlayerDodgeState
{
    public PlayerDodgeBackwardState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        
    }
    
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.BackwardHash);
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.BackwardHash);
    }
}