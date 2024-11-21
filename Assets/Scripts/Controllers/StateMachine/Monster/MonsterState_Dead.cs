
public class MonsterState_Dead : MonsterBaseState
{
    public MonsterState_Dead(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Monster.RigidBody.isKinematic = true;
        stateMachine.Monster.HitCollider.enabled = false;

        if (stateMachine.Monster.ValidAnimator)
        {
            stateMachine.Monster.AnimationController.TriggerDeath();
        }
    }


}
