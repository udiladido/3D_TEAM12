
using UnityEngine;

[System.Serializable]
public class AnimationData
{
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";

    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    
    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
    }
}