using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : BaseController
{
    public event Action<Vector2> OnMoveEvent;
    public event Action OnMoveCancelEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnDodgeEvent; 
    public event Action OnJumpEvent;
    public event Action<bool> OnRunEvent;

    private bool isRunning;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        else if (context.phase == InputActionPhase.Canceled)
            OnMoveCancelEvent?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            OnLookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            OnDodgeEvent?.Invoke();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            OnJumpEvent?.Invoke();
    }
    
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isRunning = true;
            OnRunEvent?.Invoke(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
            OnRunEvent?.Invoke(false);
        }
    }
    
    public void OnRunToggle(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isRunning = !isRunning;
            OnRunEvent?.Invoke(isRunning);
        }
    }
}