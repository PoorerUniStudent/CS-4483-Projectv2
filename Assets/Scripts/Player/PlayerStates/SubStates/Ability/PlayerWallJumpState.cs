using UnityEngine;

public class PlayerWallJumpState : PlayerAbility
{
    private int wallJumpDirection;

    private bool AttackInput;
    private bool PullInput;

    public PlayerWallJumpState(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.playerJumpingState.ResetJumps();
        player.core.Movement.SetVelocity(charData.jumpPower + charData.movementSpeed / 2,
            charData.wallJumpAngle, wallJumpDirection);
        player.core.Movement.CheckIfShouldFlip(wallJumpDirection);
        player.playerJumpingState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
        {
            return;
        }

        AttackInput = player.playerInput.attackInput;
        PullInput = player.playerInput.pullInput;

        if (AttackInput && player.playerAttackState.CheckIfCanAttack())
        {
            Transform target = core.CollisionSenses.FindNearestEnemy(false);
            if (target == null)
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
            if (target == null)
            {
                CheckNonAttackStates();
                return;
            }

            player.playerPullState.SetTarget(target);
            stateMachine.ChangeState(player.playerPullState);
        }

        player.anim.SetFloat("yVelocity", core.Movement.velocity.y);

        if (Time.time >= startTime + charData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -player.core.Movement.facingDir;
        }
        else
        {
            wallJumpDirection = player.core.Movement.facingDir;
        }
    }
}
