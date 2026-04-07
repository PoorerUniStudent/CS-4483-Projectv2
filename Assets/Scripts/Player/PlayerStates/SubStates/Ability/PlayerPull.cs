using UnityEngine;

public class PlayerPull : PlayerAbility
{
    private Transform target;
    private Rigidbody2D targetRb;
    private float timeSinceLastPull;
    private float timeSincePullStart;
    private float pullCooldown = 2f;
    public bool canPull { get; private set; }

    private float targetDistFromPlayer;
    private LineRenderer lineRenderer;
    private LineRenderer slashLine;

    public PlayerPull(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
        canPull = true;
        lineRenderer = player.lineRenderer;
        slashLine = player.slashLineupLine;
    }

    public override void Enter()
    {
        base.Enter();

        if (target == null || !canPull)
        {
            isAbilityDone = true;
            return;
        }

        canPull = false;
        // Pause all actions the enemy is doing target.FreezeActions();
        // Enemy gets pulled towards player
        targetRb = target.GetComponent<Rigidbody2D>();

        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            slashLine.enabled = false;
        }

        if (targetRb == null)
        {
            isAbilityDone = true;
            return;
        }

        core.Movement.SetVelocityZero();
        core.Movement.FreezePosition();
        Vector2 pullDirection = ((Vector2) player.transform.position - targetRb.position).normalized;

        target.GetComponent<Enemy>().SetPulledTrue();
        targetRb.linearVelocity = pullDirection * charData.pullForce;
        Debug.Log("Pulling");
        timeSincePullStart = Time.time;
    }

    public override void Exit()
    {
        base.Exit();

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }

        core.Movement.UnfreezePosition();
        timeSinceLastPull = Time.time;
        canPull = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Just in case the enemy gets stuck
        if (Time.time > timeSincePullStart + pullCooldown)
        {
            isAbilityDone = true;
        }

        targetDistFromPlayer = (target.position - player.transform.position).magnitude;

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, target.position);
        }

        Vector2 pullDirection = ((Vector2)player.transform.position - targetRb.position).normalized;
        targetRb.linearVelocity = pullDirection * charData.pullForce;

        if (targetDistFromPlayer <= 0.5f) {
            targetRb.linearVelocity = Vector2.zero;
            target.GetComponent<Enemy>().SetPulledFalse();
            isAbilityDone = true;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public bool CheckIfCanPull()
    {
        return canPull && timeSinceLastPull + pullCooldown <= Time.time;
    }
}


