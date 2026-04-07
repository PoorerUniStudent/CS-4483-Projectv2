using UnityEngine;

public class PlayerInAir : PlayerState
{
    private float InputX;
    private bool JumpInput;
    private bool JumpInputStop;
    private bool AttackInput;
    private bool PullInput;
    private bool DashInput;

    private bool isGrounded;
    private bool coyoteTime;
    private bool isJumping;

    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool isTouchingLedge;
    public bool wallJumpCoyoteTime { get; private set; }
    private float startWallJumpCoyoteTime;

    private float maxYFallVelocity = -20f;
    public PlayerInAir(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = core.CollisionSenses.Ground;
        isTouchingWall = core.CollisionSenses.Wall;
        isTouchingWallBack = core.CollisionSenses.WallBack();
        isTouchingLedge = core.CollisionSenses.CheckIfTouchingLedge();

        if (isTouchingWall && !isTouchingLedge)
        {
            player.playerLedgeClimbState.SetDetectedPosition(player.transform.position);
        }


        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        InputX = player.playerInput.moveInputRaw.x;
        JumpInput = player.playerInput.jumpInput;
        JumpInputStop = player.playerInput.jumpInputStop;
        AttackInput = player.playerInput.attackInput;
        PullInput = player.playerInput.pullInput;

        CheckJumpMultiplier();
        CheckGravity();

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
        else
        {
            CheckNonAttackStates();
        }
    }

    public override void CheckNonAttackStates()
    {
        if (isGrounded && core.Movement.velocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.playerLandingState);
        }
        /*
        else if (isTouchingWall && !isTouchingLedge && !isTouchingWallBack)
        {
            stateMachine.ChangeState(player.playerLedgeClimbState);
        }
        */
        else if (JumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = core.CollisionSenses.Wall;
            player.playerWallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.playerWallJumpState);
        }
        else if (JumpInput && player.playerJumpingState.CanJump())
        {
            coyoteTime = false;
            stateMachine.ChangeState(player.playerJumpingState);
        }
        else if (((isTouchingWall && InputX == core.Movement.facingDir) || (isTouchingWallBack && InputX != core.Movement.facingDir)) && core.Movement.velocity.y <= 0f)
        {
            stateMachine.ChangeState(player.playerWallSlideState);
        }
        else
        {
            core.Movement.CheckIfShouldFlip(InputX);
            core.Movement.SetVelocityX(player.CharData.movementSpeed * InputX);

            player.anim.SetFloat("yVelocity", core.Movement.velocity.y);
        }
    }

    private void CheckJumpMultiplier()
    {
        if (!isJumping)
        {
            return;
        }

        if (JumpInputStop)
        {
            core.Movement.SetVelocityY(core.Movement.velocity.y * charData.jumpMult);
            isJumping = false;
        }
        else if (core.Movement.velocity.y <= 0f)
        {
            isJumping = false;
        }
    }

    // Fast fall, bouncy jump
    private void CheckGravity()
    {
        // Falling
        if (core.Movement.velocity.y < 0f)
        {
            core.Movement.SetPlayerGravity(charData.defaultGravity * charData.gravityFallMult);
        }
        else
        {
            core.Movement.SetPlayerGravity(charData.defaultGravity);
        }

        if (core.Movement.velocity.y < maxYFallVelocity)
        {
            core.Movement.SetVelocityY(maxYFallVelocity);
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + charData.coyoteTime)
        {
            coyoteTime = false;
            player.playerJumpingState.DecreaseAmountOfJumpsLeft();
        }
    }
    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time >= startWallJumpCoyoteTime + charData.coyoteTime)
        {
            StartWallJumpCoyoteTime();
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;
    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }
    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;
    public void SetIsJumping() => isJumping = true;
}

