
using TMPro;
using UnityEngine;

public class MonsterState_Idle : MonsterBaseState
{
    public MonsterState_Idle(MonsterStateMachine stateMachine) : base(stateMachine) { }

    private bool doOnce;
    private Vector3 targetPosition;
    private float targetDistance;

    public override void Enter()
    {
        stateMachine.Monster.OnHit += Hit;

        doOnce = true;
        stateMachine.Monster.AnimationController.SetRun(false);
    }

    public override void Update()
    {
        targetPosition = Managers.Game.Player.transform.position;
        targetDistance = Vector3.Distance(stateMachine.Monster.transform.position, targetPosition);

        if (doOnce)
        {
            doOnce = false;

            if (targetDistance < stateMachine.Monster.Stat.attractDistance)
            {
                stateMachine.ChangeState(stateMachine.State_Chase);
            }
        }
        else
        {
            if (targetDistance < stateMachine.Monster.Stat.attractDistance)
            {
                stateMachine.ChangeState(stateMachine.State_Taunting);
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
        else
        {
            stateMachine.ChangeState(stateMachine.State_Chase);
        }
    }
}
