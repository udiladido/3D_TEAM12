public class PlayerHitState : IState
{
    protected PlayerStateMachine stateMachine;
    public PlayerHitState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    
    public void Enter()
    {
        stateMachine.Animator.SetTrigger(stateMachine.AnimationData.HitHash);
    }
    
    public void Exit()
    {
        
    }
    public void HandleInput()
    {
        
    }
    public void Update()
    {
        
    }
    public void PhysicsUpdate()
    {
        
    }
}