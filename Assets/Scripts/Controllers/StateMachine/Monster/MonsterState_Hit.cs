
using TMPro;
using UnityEngine;

public class MonsterState_Hit : MonsterBaseState
{
    public MonsterState_Hit(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Monster.OnHit += Hit;

        Look(Managers.Game.Player.transform.position);

        if (stateMachine.Monster.ValidAnimator)
        {
            stateMachine.Monster.AnimationController.OnHitEnd += StaggerEnd;

            stateMachine.Monster.AnimationController.TriggerHit();
        }
    }

    public override void Exit()
    {
        stateMachine.Monster.OnHit -= Hit;

        if (stateMachine.Monster.ValidAnimator)
        {
            stateMachine.Monster.AnimationController.OnHitEnd -= StaggerEnd;
        }
    }


    private void StaggerEnd()
    {
        stateMachine.ChangeState(stateMachine.State_Chase);
    }

    private void Hit(bool isStagger)
    {
        if (isStagger)
        {
            stateMachine.ChangeState(stateMachine.State_Hit);
        }
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
