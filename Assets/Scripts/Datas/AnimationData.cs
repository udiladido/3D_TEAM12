using UnityEngine;

[System.Serializable]
public class AnimationData
{
    [SerializeField] private string runParameterName = "@Run";
    [SerializeField] private string dodgeParameterName = "Dodge";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string forwardParameterName = "Forward";
    [SerializeField] private string backwardParameterName = "Backward";
    [SerializeField] private string leftParameterName = "LeftStep";
    [SerializeField] private string rightParameterName = "RightStep";
    [SerializeField] private string deadParameterName = "Dead";

    public int RunHash { get; private set; }
    public int DodgeHash { get; private set; }
    public int IdleHash { get; private set; }
    public int ForwardHash { get; private set; }
    public int BackwardHash { get; private set; }
    public int LeftHash { get; private set; }
    public int RightHash { get; private set; }
    public int DeadHash { get; private set; }

    public void Initialize()
    {
        RunHash = Animator.StringToHash(runParameterName);
        DodgeHash = Animator.StringToHash(dodgeParameterName);
        IdleHash = Animator.StringToHash(idleParameterName);
        ForwardHash = Animator.StringToHash(forwardParameterName);
        BackwardHash = Animator.StringToHash(backwardParameterName);
        LeftHash = Animator.StringToHash(leftParameterName);
        RightHash = Animator.StringToHash(rightParameterName);
        DeadHash = Animator.StringToHash(deadParameterName);
    }
}