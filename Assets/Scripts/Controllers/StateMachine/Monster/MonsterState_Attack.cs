using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterState_Attack : MonsterBaseState
{
    public MonsterState_Attack(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        transform = stateMachine.Monster.transform;
    }

    private Transform transform;
    private bool triggered;
    private float attackTimeDelay;

    public override void Enter()
    {
        stateMachine.Monster.OnHit += Hit;

        if (stateMachine.Monster.ValidAnimator && stateMachine.Monster.ValidSkill)
        {
            triggered = stateMachine.Monster.AnimationController.TriggerAttack(stateMachine.Monster.NextSkillIndex);
            if (triggered)
            {
                attackTimeDelay = stateMachine.Monster.Stat.skillEntities[stateMachine.Monster.NextSkillIndex].attackPeriod;
            }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.State_Chase);
        }
    }

    public override void Update()
    {
        Look(Managers.Game.Player.transform.position);

        if (triggered)
        {
            attackTimeDelay -= Time.deltaTime;
            if (attackTimeDelay < 0)
            {
                stateMachine.Monster.SetNextSkill();

                float targetDistance = Vector3.Distance(stateMachine.Monster.transform.position, Managers.Game.Player.transform.position);

                if (targetDistance > stateMachine.Monster.NextSkillRange)
                {
                    stateMachine.ChangeState(stateMachine.State_Chase);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.State_Attack);
                }
            }
        }
        else
        {
            if (stateMachine.Monster.ValidAnimator)
            {
                triggered = stateMachine.Monster.AnimationController.TriggerAttack(stateMachine.Monster.NextSkillIndex);
                if (triggered)
                {
                    attackTimeDelay = stateMachine.Monster.Stat.skillEntities[stateMachine.Monster.NextSkillIndex].attackPeriod;
                }
            }
        }
    }

    public override void Exit()
    {
        stateMachine.Monster.OnHit -= Hit;
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
        Vector3 vectorLook = targetPos - transform.position;
        vectorLook.y = 0f;
        if (vectorLook != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(vectorLook);
            transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        }
    }
}
