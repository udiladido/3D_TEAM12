public class PlayerStateMachine : StateMachine
{
    public Player Player { get; private set; }
    
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
}