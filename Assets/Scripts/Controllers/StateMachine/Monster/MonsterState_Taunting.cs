
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

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
            if (triggered)
            {
                Look(Managers.Game.Player.transform.position);
            }
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


    // Y축 회전만 적용
    private void Look(Vector3 targetPos)
    {
        Vector3 vectorLook = targetPos - stateMachine.Monster.transform.position;
        vectorLook.y = 0f;
        if (vectorLook != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(vectorLook);
            stateMachine.Monster.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        }
    }
}
