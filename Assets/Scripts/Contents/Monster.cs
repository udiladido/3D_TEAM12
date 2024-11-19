using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [field: SerializeField] private MonsterEntity currentMonster;
    [field: SerializeField] private MonsterCondition Condition;
    [field: SerializeField] private MonsterAnimatorController AnimationController;

    public int Identifier { get; private set; }
    //private PlayerStateMachine stateMachine;

    private void Awake()
    {
        Condition = GetComponent<MonsterCondition>();

        //stateMachine = new PlayerStateMachine(this);
    }

    private void Initialize(MonsterEntity monsterEntity)
    {
        currentMonster = monsterEntity;

        Condition.SetData(monsterEntity.maxHp);

        AnimationController = GetComponentInChildren<MonsterAnimatorController>();
        AnimationController.Initialize();
    }

    private void FixedUpdate()
    {
        //stateMachine.PhysicsUpdate();
    }

    public void Die()
    {
        Condition.OnHit -= Hit;
        Condition.OnDead -= Die;
        //stateMachine.ChangeState(stateMachine.DeadState);
    }

    public void Hit()
    {
        //stateMachine.Hit();
    }
}
