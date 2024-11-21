using System;
using UnityEngine;

public class MonsterState_Chase : MonsterBaseState
{
    public MonsterState_Chase(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        transform = stateMachine.Monster.transform;
    }

    private Transform transform;
    private Vector3 targetPosition;
    private float targetDistance;

    private float chaseTimeRemain;

    public override void Enter()
    {
        stateMachine.Monster.OnHit += Hit;

        chaseTimeRemain = stateMachine.Monster.Stat.chasePeriod;

        if (stateMachine.Monster.ValidAnimator)
        {
            stateMachine.Monster.AnimationController.SetRun(true);

        }
    }

    public override void Update()
    {
        targetPosition = Managers.Game.Player.transform.position;
        targetDistance = Vector3.Distance(transform.position, targetPosition);

        if (chaseTimeRemain >= 0) chaseTimeRemain -= Time.deltaTime;

        // 추적 중단
        if (chaseTimeRemain < 0 && targetDistance > stateMachine.Monster.Stat.attractDistance)
        {
            stateMachine.ChangeState(stateMachine.State_Idle);
        }
        // 이동
        else if (targetDistance > stateMachine.Monster.NextSkillRange)
        {
            // Look, Y축 회전만 적용
            Vector3 vectorLook = targetPosition - transform.position;
            vectorLook.y = 0f;
            if (vectorLook != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(vectorLook);
                transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            }

            // Move
            transform.Translate(stateMachine.Monster.Stat.moveSpeed * transform.forward * Time.deltaTime, Space.World);
        }
        // 공격 개시
        else
        {
            stateMachine.ChangeState(stateMachine.State_Attack);
        }
    }

    public override void Exit()
    {
        stateMachine.Monster.OnHit -= Hit;

        if (stateMachine.Monster.ValidAnimator)
        {
            stateMachine.Monster.AnimationController.SetRun(false);
        }
    }


    private void Hit(bool isStagger)
    {
        if (isStagger)
        {
            stateMachine.ChangeState(stateMachine.State_Hit);
        }
        else
        {
            chaseTimeRemain = stateMachine.Monster.Stat.chasePeriod;
        }
    }
}
