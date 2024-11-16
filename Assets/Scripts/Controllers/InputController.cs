using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : BaseController
{
    public event Action<Vector2> OnMoveEvent;
    public event Action OnMoveCancelEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnDodgeEvent; 

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
}