using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public Vector2 moveInputRaw { get; private set; }
    public bool jumpInput { get; private set; }
    public bool jumpInputStop { get; private set; }
    private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;
    public bool attackInput { get; private set; }
    public bool interactInput { get; private set; }
    public bool pullInput { get; private set; }
    public bool dropInput {  get; private set; }

    public UnityEvent onEscOpen, onEscClose;
    private bool isPauseMenuOpen;

    private void Update()
    {
        CheckJumpInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInputRaw = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpInput = true;
            jumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            jumpInputStop = true;
        }
    }
    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            jumpInput = false;
        }
    }

    public void UseJumpInput() { jumpInput = false; }

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

    public void UseAttackInput()
    {
        attackInput = false;
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactInput = true;
        }

        if (context.canceled)
        {
            interactInput = false;
        }
    }

    public void UseInteractInput()
    {
        interactInput = false;
    }

    public void OnPullInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pullInput = true;
        }

        if (context.canceled)
        {
            pullInput = false;
        }
    }

    public void UsePullInput()
    {
        pullInput = false;
    }

    public void OnDropInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            dropInput = true;
        }

        if (context.canceled)
        {
            dropInput = false;
        }
    }

    public void OnEscInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isPauseMenuOpen = !isPauseMenuOpen;

            if (isPauseMenuOpen)
            {
                onEscOpen.Invoke();
            }
            else 
            {
                onEscClose.Invoke();
            }
        }
    }
}
