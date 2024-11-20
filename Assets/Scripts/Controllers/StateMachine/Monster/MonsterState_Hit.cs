
using TMPro;
using UnityEngine;

public class MonsterState_Hit : MonsterBaseState
{
    public MonsterState_Hit(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        transform = stateMachine.Monster.transform;
    }

    private Transform transform;

    public override void Enter()
    {
        stateMachine.Monster.OnHit += Hit;

        // Look, Y축 회전만 적용
        Vector3 vectorLook = Managers.Game.Player.transform.position - transform.position;
        vectorLook.y = 0f;
        if (vectorLook != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(vectorLook);
            transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        }

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
}
