using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public Vector2 moveInputRaw { get; private set; }
    public bool jumpInput { get; private set; }
    public bool attackInput { get; private set; }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInputRaw = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpInput = true;
        }

        if (context.performed)
        {
            
        }
        
        if (context.canceled)
        {
            
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            attackInput = true;
        }
        
        if (context.canceled)
        {
            attackInput = false;
        }
    }
}
