using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public float NormInputX { get; private set; }
    public bool JumpInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool AttackInput { get; private set; }

    public void Update()
    {
        Debug.Log(NormInputX);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        NormInputX = context.ReadValue<float>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
        }
        if (context.canceled)
        {
            JumpInput = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
        }
        if (context.canceled)
        {
            DashInput = false;
        }

    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
        }
        
        if (context.canceled)
        {
            AttackInput = false;
        }

    }
    public void UseJumpInput() => JumpInput = false;
}
