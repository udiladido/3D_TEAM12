using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Condition stat;
    private InputController input;
    private MoveController move;
    [field: SerializeField] public AnimationData AnimationData { get; private set; }
    
    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        stat = GetComponent<Condition>();
        input = GetComponent<InputController>();
        move = GetComponent<MoveController>();
    }

}