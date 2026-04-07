using UnityEngine;

public class PlayerLedgeClimbState : PlayerAbility
{
    private Vector2 detectedPosition;
    private Vector2 cornerPosition;
    private Vector2 startPos;
    private Vector2 stopPos;

    private bool climbedUp;
    public PlayerLedgeClimbState(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        climbedUp = true;
        player.transform.position = new Vector2(startPos.x, stopPos.y - charData.stopOffset.y * 0.9f);
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        player.transform.position = detectedPosition;
        cornerPosition = core.CollisionSenses.DetermineCornerPosition();

        startPos.Set(cornerPosition.x - (core.Movement.facingDir * charData.startOffset.x), cornerPosition.y - charData.startOffset.y);
        stopPos.Set(cornerPosition.x + (core.Movement.facingDir * charData.stopOffset.x), cornerPosition.y + charData.stopOffset.y);

        climbedUp = false;
        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();

        player.transform.position = stopPos;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
        else if (!climbedUp)
        {
            player.transform.position = startPos;
        }

        core.Movement.SetVelocityZero();
    }

    public void SetDetectedPosition(Vector2 position)
    {
        detectedPosition = position;
    }
}
