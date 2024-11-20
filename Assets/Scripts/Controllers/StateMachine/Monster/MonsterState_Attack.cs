using UnityEngine;

public class MonsterState_Attack : MonsterBaseState
{
    public MonsterState_Attack(MonsterStateMachine stateMachine) : base(stateMachine) { }

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
        if (triggered)
        {
            attackTimeDelay -= Time.deltaTime;
            if (attackTimeDelay < 0)
            {
                stateMachine.ChangeState(stateMachine.State_Chase);
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
}
