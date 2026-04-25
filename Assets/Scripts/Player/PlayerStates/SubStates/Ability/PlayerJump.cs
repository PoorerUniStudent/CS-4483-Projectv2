using UnityEngine;

public class PlayerJump : PlayerAbility
{
    private int jumpsLeft;
    public PlayerJump(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
        jumpsLeft = charData.jumps;
    }
    public override void Enter()
    {
        base.Enter();

        player.playerInput.UseJumpInput();
        core.Movement.SetVelocityY(charData.jumpPower);
        isAbilityDone = true;
        jumpsLeft--;
        player.playerInAirState.SetIsJumping();

        SFXManager.instance.PlaySFX(1);
    }

    public void ResetJumps() => jumpsLeft = charData.jumps;
    public void DecreaseAmountOfJumpsLeft() => jumpsLeft--;
    public bool CanJump()
    {
        return jumpsLeft > 0;
    }
}
