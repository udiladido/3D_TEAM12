
using UnityEditor.Animations;

public class MonsterState_Taunting : MonsterBaseState
{
    public MonsterState_Taunting(MonsterStateMachine stateMachine) : base(stateMachine) { }

    private bool animatorExist;
    private bool triggered;

    public override void Enter()
    {
        if (stateMachine.Monster.ValidAnimator)
        {
            stateMachine.Monster.AnimationController.OnTauntingEnd += TauntingEnd;

            triggered = stateMachine.Monster.AnimationController.TriggerTaunt();
        }
    }

    public override void Update()
    {
        if (stateMachine.Monster.ValidAnimator && triggered == false)
        {
            triggered = stateMachine.Monster.AnimationController.TriggerTaunt();
        }
    }

    public override void Exit()
    {
        if (stateMachine.Monster.ValidAnimator)
        {
            stateMachine.Monster.AnimationController.OnTauntingEnd -= TauntingEnd;
        }
    }


    private void TauntingEnd()
    {
        stateMachine.ChangeState(stateMachine.State_Chase);
    }
}
