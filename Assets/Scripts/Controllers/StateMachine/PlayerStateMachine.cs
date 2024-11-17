using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; private set; }
    public Animator Animator => Player?.Animator;
    public AnimationData AnimationData => Player?.AnimationData;
    public Condition Condition => Player?.Condition;
    public InputController Input => Player?.Input;
    public CharacterController Controller => Player?.Controller;
    public Combat Combat => Player?.Combat;
    
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerDodgeState DodgeState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    
    public PlayerHitState HitState { get; private set; }

    public Vector3 LookDirection { get; set; }
    public Vector3 MoveDirection { get; set; }

    public Defines.CharacterMovementType MovementType { get; set; }
    public bool IsDodging { get; set; }
    public bool IsFalling { get; set; }
    public bool IsRunnung { get; set; }
    
    public bool IsAttacking { get; set; }
    public Defines.CharacterAttackInputType AttackInputType { get; set; }
    
    public int JumpCount { get; set; }

    public PlayerStateMachine(Player player)
    {
        Player = player;
        IdleState = new PlayerIdleState(this);
        RunState = new PlayerRunState(this);
        DodgeState = new PlayerDodgeState(this);
        DeadState = new PlayerDeadState(this);
        JumpState = new PlayerJumpState(this);
        FallState = new PlayerFallState(this);
        HitState = new PlayerHitState(this);
    }

    public void Hit()
    {
        ChangeState(HitState);
    }
}