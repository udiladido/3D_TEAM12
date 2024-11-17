using UnityEngine;

[System.Serializable]
public class AnimationData
{
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string runParameterName = "@Run";
    [SerializeField] private string dodgeParameterName = "Dodge";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string forwardParameterName = "Forward";
    [SerializeField] private string backwardParameterName = "Backward";
    [SerializeField] private string leftParameterName = "LeftStep";
    [SerializeField] private string rightParameterName = "RightStep";
    [SerializeField] private string deadParameterName = "Dead";
    
    [SerializeField] private string runBlendParameterName = "RunBlend";
    
    [SerializeField] private string airParameterName = "@Air";
    [SerializeField] private string jumpParameterName = "Jump";
    [SerializeField] private string fallParameterName = "Fall";

    [SerializeField] private string attackParameterName = "@Attack";
    [SerializeField] private string comboAttackParameterName = "@ComboAttack";
    [SerializeField] private string comboAttackIndexParameterName = "@ComboAttackIndex";
    
    
    [SerializeField] private string hitParameterName = "@Hit";
    
    public int GroundHash { get; private set; }
    public int RunHash { get; private set; }
    public int DodgeHash { get; private set; }
    public int IdleHash { get; private set; }
    public int ForwardHash { get; private set; }
    public int BackwardHash { get; private set; }
    public int LeftHash { get; private set; }
    public int RightHash { get; private set; }
    public int DeadHash { get; private set; }
    public int RunBlendHash { get; private set; }
    
    public int AirHash { get; private set; }
    public int JumpHash { get; private set; }
    public int FallHash { get; private set; }
    
    public int AttackHash { get; private set; }
    public int ComboAttackHash { get; private set; }
    public int ComboAttackIndexHash { get; private set; }
    
    public int HitHash { get; private set; }

    public void Initialize()
    {
        GroundHash = Animator.StringToHash(groundParameterName);
        RunHash = Animator.StringToHash(runParameterName);
        DodgeHash = Animator.StringToHash(dodgeParameterName);
        IdleHash = Animator.StringToHash(idleParameterName);
        ForwardHash = Animator.StringToHash(forwardParameterName);
        BackwardHash = Animator.StringToHash(backwardParameterName);
        LeftHash = Animator.StringToHash(leftParameterName);
        RightHash = Animator.StringToHash(rightParameterName);
        DeadHash = Animator.StringToHash(deadParameterName);
        RunBlendHash = Animator.StringToHash(runBlendParameterName);
        AirHash = Animator.StringToHash(airParameterName);
        JumpHash = Animator.StringToHash(jumpParameterName);
        FallHash = Animator.StringToHash(fallParameterName);
        AttackHash = Animator.StringToHash(attackParameterName);
        ComboAttackHash = Animator.StringToHash(comboAttackParameterName);
        ComboAttackIndexHash = Animator.StringToHash(comboAttackIndexParameterName);
        HitHash = Animator.StringToHash(hitParameterName);
    }
}