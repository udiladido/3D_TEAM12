public class PlayerAttackState : IState
{
    protected PlayerStateMachine stateMachine;

    public PlayerAttackState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        stateMachine.Animator.SetTrigger(stateMachine.AnimationData.AttackHash);
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