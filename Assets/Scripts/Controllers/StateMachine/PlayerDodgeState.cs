public class PlayerDodgeState : PlayerBaseState
{
    public PlayerDodgeState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        
    }
    
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.DodgeHash);
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.DodgeHash);
    }
}