
public class MonsterState_Spawning : MonsterBaseState
{
    public MonsterState_Spawning(MonsterStateMachine stateMachine) : base(stateMachine) { }

    private bool triggered;

    public override void Enter()
    {
        stateMachine.Monster.RigidBody.isKinematic = true;
        stateMachine.Monster.HitCollider.enabled = false;

        if (stateMachine.Monster.ValidAnimator)
        {
            stateMachine.Monster.AnimationController.OnSpawningEnd += SpawningEnd;

            triggered = stateMachine.Monster.AnimationController.TriggerSpawn();
        }
    }

    public override void Update()
    {
        if (stateMachine.Monster.ValidAnimator && triggered == false)
        {
            triggered = stateMachine.Monster.AnimationController.TriggerSpawn();
        }
    }

    public override void Exit()
    {
        stateMachine.Monster.HitCollider.enabled = true;
        stateMachine.Monster.RigidBody.isKinematic = false;

        if (stateMachine.Monster.ValidAnimator)
        {
            stateMachine.Monster.AnimationController.OnSpawningEnd -= SpawningEnd;
        }
    }


    private void SpawningEnd()
    {
        stateMachine.ChangeState(stateMachine.State_Idle);
    }
}
