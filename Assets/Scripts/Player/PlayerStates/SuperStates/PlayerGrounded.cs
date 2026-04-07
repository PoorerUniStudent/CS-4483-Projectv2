using UnityEngine;

public class PlayerGrounded : PlayerState
{
    protected float InputX;
    protected bool JumpInput;
    protected bool AttackInput;
    protected bool PullInput;
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
        PullInput = player.playerInput.pullInput;

        isGrounded = core.CollisionSenses.Ground;

        if (AttackInput && player.playerAttackState.CheckIfCanAttack())
        {
            Transform target = core.CollisionSenses.FindNearestEnemy(false);
            if (target == null || target.GetComponent<Enemy>().dead)
            {
                CheckNonAttackStates();
                return;
            }

            player.playerAttackState.SetTarget(target);
            stateMachine.ChangeState(player.playerAttackState);
        }
        else if (PullInput && player.playerPullState.CheckIfCanPull())
        {
            Transform target = core.CollisionSenses.FindNearestEnemy(true);
            if (target == null || target.GetComponent<Enemy>().dead)
            {
                CheckNonAttackStates();
                return;
            }

            player.playerPullState.SetTarget(target);
            stateMachine.ChangeState(player.playerPullState);
        }
        else
        {
            CheckNonAttackStates();
        }
    }

    public override void CheckNonAttackStates()
    {
        if (JumpInput && player.playerJumpingState.CanJump())
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
