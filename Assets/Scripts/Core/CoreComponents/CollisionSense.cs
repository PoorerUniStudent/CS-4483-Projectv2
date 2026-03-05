using UnityEngine;

public class CollisionSense : CoreComponent
{
    public Transform GroundCheck { get => groundCheck; private set => groundCheck = value; }
    public float GroundCheckRadius { get => groundCheckRadius; private set => groundCheckRadius = value; }
    public LayerMask WhatIsGround { get => whatIsGround; private set => whatIsGround = value; }

    public Transform WallCheck { get => wallCheck; private set => wallCheck = value; }
    public float WallCheckDistance { get => wallCheckDistance; private set => wallCheckDistance = value; }
    public LayerMask WhatIsWall { get => whatIsWall; private set => whatIsWall = value; }
    public Transform LedgeCheck { get => ledgeCheck; private set => ledgeCheck = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsWall;

    [SerializeField] private Transform ledgeCheck;

    public bool Ground
    {
        get => Physics2D.OverlapBox(GroundCheck.position, new Vector2(groundCheckRadius, groundCheckRadius), 0f, whatIsGround);
    }


    public bool Wall
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * core.Movement.facingDir, wallCheckDistance, whatIsWall);
    }

    public bool WallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -core.Movement.facingDir, wallCheckDistance, whatIsWall);
    }

    void OnDrawGizmos()
    {
        if (core == null)
        {
            return;
        }

        Gizmos.DrawWireCube(GroundCheck.position, new Vector2(groundCheckRadius, groundCheckRadius));
        Debug.DrawRay(wallCheck.position, Vector2.right * core.Movement.facingDir * wallCheckDistance, Color.red);
        Debug.DrawRay(wallCheck.position, Vector2.right * -core.Movement.facingDir * wallCheckDistance, Color.red);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * core.Movement.facingDir, wallCheckDistance, whatIsWall);
    }

    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * core.Movement.facingDir, wallCheckDistance, whatIsWall);
        float xDistance = xHit.distance;

        Vector2 cornerPos = new Vector2(xDistance * core.Movement.facingDir, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)cornerPos, Vector2.down,
            ledgeCheck.position.y - wallCheck.position.y, whatIsWall);
        float yDistance = yHit.distance;

        cornerPos.Set(core.CollisionSenses.WallCheck.position.x + (xDistance * core.Movement.facingDir),
            core.CollisionSenses.LedgeCheck.position.y - yDistance);

        return cornerPos;
    }
}
