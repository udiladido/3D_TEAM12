using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : BaseController
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            OnMoveEvent?.Invoke(inputVector);
            Debug.Log(inputVector);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            OnMoveEvent?.Invoke(Vector2.zero);
            Debug.Log("Stop Input");
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            OnLookEvent?.Invoke(context.ReadValue<Vector2>());
    }
}