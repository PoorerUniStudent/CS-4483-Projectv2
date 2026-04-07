using UnityEngine;

public class PlayerWallSlideState : PlayerOnWall
{
    public PlayerWallSlideState(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
        {
            return;
        }

        player.core.Movement.SetVelocityY(-charData.wallSlideVelocity);
    }
}
