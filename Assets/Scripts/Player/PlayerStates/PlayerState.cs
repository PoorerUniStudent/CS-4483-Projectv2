using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState
{
    protected Core core; // Methods for movement and collision detection

    protected Player player;
    protected PlayerFiniteStateMachine stateMachine;
    protected CharacterData charData;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    protected Vector2 mouseWorldPos;

    protected bool InteractInput;

    private string animBoolName;

    public PlayerState(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.charData = charData;
        this.animBoolName = animBoolName;
        core = player.core;
    }

    public virtual void Enter()
    {
        DoChecks();
        player.anim.SetBool(animBoolName, true);
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;

        Debug.Log(animBoolName);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {
        DoChecks();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        InteractInput = player.playerInput.interactInput;

        if (InteractInput)
        {
            foreach (GameObject obj in player.touchList)
            {
                Interactable interactable = obj.GetComponent<Interactable>();
                if (interactable)
                {
                    player.playerInput.UseInteractInput();
                    interactable.Interact();
                }
            }
        }
    } // Updates every Update()

    public virtual void DoChecks() { } // Ground check, ledge check etc.

    public virtual void AnimationTrigger() { } // Triggered in anim

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true; // Triggered in anim
}
