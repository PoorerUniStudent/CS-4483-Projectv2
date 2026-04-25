using UnityEngine;
using UnityEngine.Windows;

public class PlayerMoving : PlayerGrounded
{
    public PlayerMoving(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.Movement.CheckIfShouldFlip(InputX);
        core.Movement.SetVelocityX(charData.movementSpeed * InputX);

        if (!isExitingState)
        {
            if (InputX == 0)
            {
                stateMachine.ChangeState(player.playerIdleState);
            }
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        SFXManager.instance.PlaySFX(0);
    }
}
