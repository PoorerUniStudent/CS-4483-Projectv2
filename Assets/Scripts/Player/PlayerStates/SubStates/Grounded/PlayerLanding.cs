using UnityEngine;
using UnityEngine.Windows;

public class PlayerLanding : PlayerGrounded
{
    public PlayerLanding(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (InputX != 0)
            {
                stateMachine.ChangeState(player.playerRunningState);
            }
            else // if (isAnimationFinished)
            {
                stateMachine.ChangeState(player.playerIdleState);
            }
        }
    }
}