using UnityEngine;

public class PlayerGrounded : PlayerState
{
    protected float InputX;
    protected bool JumpInput;
    protected bool AttackInput;
    private bool DashInput;

    private bool isGrounded;
    public PlayerGrounded(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        // Check if grounded
        isGrounded = core.CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        player.playerJumpingState.ResetJumps();
        core.Movement.SetPlayerGravity(charData.defaultGravity);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        InputX = player.playerInput.moveInputRaw.x;
        JumpInput = player.playerInput.jumpInput;
        AttackInput = player.playerInput.attackInput;
        isGrounded = core.CollisionSenses.Ground;
        if (AttackInput)
        {
            stateMachine.ChangeState(player.playerAttackState);
        } else if (JumpInput && player.playerJumpingState.CanJump())
        {
            stateMachine.ChangeState(player.playerJumpingState);
        }
        else if (!isGrounded)
        {
            player.playerInAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.playerInAirState);
        }
    }
}
