using System.Collections;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D rb { get; private set; }
    public Vector2 velocity { get; private set; }
    public int facingDir { get; private set; }
    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponentInParent<Rigidbody2D>();

        facingDir = 1;
    }

    public void SetPlayerGravity(float gravity)
    {
        rb.gravityScale = gravity;
    }

    public void LogicUpdate()
    {
        velocity = rb.linearVelocity;
    }

    #region Set Functions

    public void SetVelocityZero()
    {
        workspace = Vector2.zero;
        SetFinalVelocity();
    }

    // For dashing/recoil
    public void SetVelocity(float v, Vector2 angle, int direction)
    {
        // v = speed
        angle.Normalize();
        workspace.Set(angle.x * v * direction, angle.y * v);
        SetFinalVelocity();
    }
    public void SetVelocity(float v, Vector2 angle)
    {
        // v = speed
        angle.Normalize();
        workspace.Set(angle.x * v, angle.y * v);
        SetFinalVelocity();
    }

    public void SetVelocityX(float v)
    {
        workspace.Set(v, velocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float v)
    {
        workspace.Set(velocity.x, v);
        SetFinalVelocity();
    }

    public void SetDashVelocity(float v, Vector2 angle)
    {
        SetVelocityZero();

        angle.Normalize();
        workspace.Set(angle.x * v, 0f);
        SetFinalVelocity();
    }

    private void SetFinalVelocity()
    {
        rb.linearVelocity = workspace;
        velocity = workspace;
    }

    public void CheckIfShouldFlip(float xInput)
    {
        int xInputInt = Mathf.CeilToInt(xInput);
        if (xInput < 0)
        {
            xInputInt = Mathf.FloorToInt(xInput);
        }

        if (xInputInt != 0 && xInputInt != facingDir)
        {
            Flip();
        }
    }

    public void Flip()
    {
        facingDir *= -1;
        rb.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}