using UnityEngine;
using UnityEngine.Windows;

public class PlayerIdle : PlayerGrounded
{
    public PlayerIdle(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityX(0f);
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
        }
    }
}
