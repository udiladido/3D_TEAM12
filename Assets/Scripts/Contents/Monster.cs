using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [field: SerializeField] public Condition Condition { get; private set; }
    //[field: SerializeField] public AnimationData AnimationData { get; private set; }
    public Animator Animator { get; private set; }

    //private PlayerStateMachine stateMachine;

    private void Awake()
    {
        Condition = GetComponent<Condition>();
        Animator = GetComponentInChildren<Animator>();

        //stateMachine = new PlayerStateMachine(this);
        //AnimationData.Initialize();
    }

    private void OnEnable()
    {
        
    }

    private void FixedUpdate()
    {
        //stateMachine.PhysicsUpdate();
    }

    public void Die()
    {
        Condition.OnDead -= Die;
        Condition.OnHit -= Hit;
        //stateMachine.ChangeState(stateMachine.DeadState);
    }

    public void Hit()
    {
        //stateMachine.Hit();
    }
}
