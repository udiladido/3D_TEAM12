
public class MonsterStateMachine : StateMachine
{
    public Monster Monster { get; private set; }

    public MonsterBaseState State_Idle { get; private set; }
    public MonsterBaseState State_Spawning { get; private set; }
    public MonsterBaseState State_Taunting { get; private set; }
    public MonsterBaseState State_Chase { get; private set; }
    public MonsterBaseState State_Attack { get; private set; }
    public MonsterBaseState State_Hit { get; private set; }
    public MonsterBaseState State_Dead { get; private set; }

    public MonsterStateMachine(Monster monster)
    {
        this.Monster = monster;

        State_Idle = new MonsterState_Idle(this);
        State_Spawning = new MonsterState_Spawning(this);
        State_Taunting = new MonsterState_Taunting(this);
        State_Chase = new MonsterState_Chase(this);
        State_Attack = new MonsterState_Attack(this);
        State_Dead = new MonsterState_Dead(this);
    }


}
