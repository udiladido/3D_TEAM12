using UnityEngine;

public class PlayerDodgeState : PlayerRunState
{
    private Defines.CharacterMovementType currentMovementType;
    public PlayerDodgeState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.Input.OnMoveEvent += MoveHandle;
        stateMachine.Input.OnMoveCancelEvent += MoveCancelHandle;
        stateMachine.Input.OnLookEvent += LookHandle;
        StartAnimation(stateMachine.AnimationData.GroundHash);
        StartAnimation(stateMachine.AnimationData.RunHash);
        StartAnimation(stateMachine.AnimationData.DodgeHash);
        currentMovementType = stateMachine.MovementType;
        switch (currentMovementType)
        {
            case Defines.CharacterMovementType.Backward:
                StartAnimation(stateMachine.AnimationData.BackwardHash);
                break;
            case Defines.CharacterMovementType.LeftStep:
                StartAnimation(stateMachine.AnimationData.LeftHash);
                break;
            case Defines.CharacterMovementType.RightStep:
                StartAnimation(stateMachine.AnimationData.RightHash);
                break;
            case Defines.CharacterMovementType.Forward:
                StartAnimation(stateMachine.AnimationData.ForwardHash);
                break;
        }
        // TODO : 재사용 대기시간 설정
        TryApplyForce();
    }

    public override void Exit()
    {
        stateMachine.Input.OnMoveEvent -= MoveHandle;
        stateMachine.Input.OnMoveCancelEvent -= MoveCancelHandle;
        stateMachine.Input.OnLookEvent -= LookHandle;
        stateMachine.IsDodging = false;
        StopAnimation(stateMachine.AnimationData.GroundHash);
        StopAnimation(stateMachine.AnimationData.RunHash);
        StopAnimation(stateMachine.AnimationData.DodgeHash);
        switch (currentMovementType)
        {
            case Defines.CharacterMovementType.Backward:
                StopAnimation(stateMachine.AnimationData.BackwardHash);
                break;
            case Defines.CharacterMovementType.LeftStep:
                StopAnimation(stateMachine.AnimationData.LeftHash);
                break;
            case Defines.CharacterMovementType.RightStep:
                StopAnimation(stateMachine.AnimationData.RightHash);
                break;
            case Defines.CharacterMovementType.Forward:
                StopAnimation(stateMachine.AnimationData.ForwardHash);
                break;
        }
    }

    private void TryApplyForce()
    {
        if (stateMachine.IsDodging) return;
        stateMachine.IsDodging = true;

        stateMachine.Player.ForceReceiver.Reset();
        stateMachine.Player.ForceReceiver.AddForce(stateMachine.MoveDirection * stateMachine.Player.DodgeForce);
    }

    public override void Update()
    {
        base.Update();

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Dodge");
        if (normalizedTime >= 1f)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
    }
}