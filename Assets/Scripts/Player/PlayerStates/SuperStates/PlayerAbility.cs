using UnityEngine;

public class PlayerAbility : PlayerState
{
    protected bool isAbilityDone;

    private bool isGrounded;
    public PlayerAbility(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isGrounded = core.CollisionSenses.Ground;

        if (isAbilityDone)
        {
            if (isGrounded && core.Movement.velocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.playerIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.playerInAirState);
            }
        }
    }
}
