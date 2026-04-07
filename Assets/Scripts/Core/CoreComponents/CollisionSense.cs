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

    public LayerMask WhatIsEnemy { get => whatIsEnemy; private set => whatIsEnemy = value; }
    public float PullCheckRadius { get => pullCheckRadius; private set => pullCheckRadius = value; }
    public Transform PlatformCheck { get => platformCheck; private set => platformCheck = value; }
    public LayerMask WhatIsPlatform { get => whatIsPlatform; private set => whatIsPlatform = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyCheckRadius;
    [SerializeField] private float pullCheckRadius;
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsWall;

    [SerializeField] private Transform ledgeCheck;

    [SerializeField] private Transform platformCheck;
    [SerializeField] private float platformCheckDistance;
    [SerializeField] private LayerMask whatIsPlatform;

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

    public Collider2D Platform
    {
        get => Physics2D.OverlapBox(PlatformCheck.position, new Vector2(platformCheckDistance, platformCheckDistance), 0f, whatIsPlatform);
    }

    public Transform FindNearestEnemy(bool usePullRange)
    {
        float radius = enemyCheckRadius;

        if (usePullRange)
        {
            radius = pullCheckRadius;
        }

        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(enemyCheck.position, radius, whatIsEnemy);
        float closestDistance = 999f;
        Transform closestEnemy = null;

        foreach (Collider2D enemy in nearbyEnemies)
        {
            if ((enemyCheck.position - enemy.transform.position).magnitude < closestDistance)
            {
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    void OnDrawGizmos()
    {
        if (core == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(enemyCheck.position, pullCheckRadius);
        Gizmos.DrawWireCube(GroundCheck.position, new Vector2(groundCheckRadius, groundCheckRadius));
        Gizmos.DrawWireCube(PlatformCheck.position, new Vector2(platformCheckDistance, platformCheckDistance));
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
