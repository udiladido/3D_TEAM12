public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleHash);
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleHash);
    }
    
    public override void Update()
    {
        base.Update();
        
    }
}