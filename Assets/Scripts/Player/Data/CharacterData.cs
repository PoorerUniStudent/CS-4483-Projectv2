using UnityEngine;

[CreateAssetMenu(fileName = "newCharacterData", menuName = "Data/Character Data/Base Data")]
public class CharacterData : ScriptableObject
{
    [Header("Movement")]
    public float movementSpeed = 10.0f;
    public float jumpPower = 10.0f;
    public int jumps = 1;
    /*
    public float dashTime = 0.5f;
    public float dashCooldown = 1.5f;
    public int dashes = 1;
    */

    // Const
    public float coyoteTime = 0.2f;
    public float jumpMult = 0.7f;
    public float defaultGravity = 2.5f;
    public float gravityFallMult = 2f;

    [Header("Abilities")]
    public float dashForce = 15f;
    public float pullForce = 20f;

    [Header("On Wall State")]
    public float wallSlideVelocity = 3f;
    public float wallJumpVelocity = 10f;
    public float wallJumpTime = 0.3f;
    public Vector2 wallJumpAngle = new Vector2(1f, 2f);

    [Header("Ledge Climb State")]
    public Vector2 startOffset;
    public Vector2 stopOffset;
}

