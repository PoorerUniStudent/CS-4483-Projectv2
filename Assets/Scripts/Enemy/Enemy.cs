using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int health;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;

    [Header("Player Detection")]
    [SerializeField] protected float detectDistance;
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Wander")]
    [SerializeField] protected float wanderDistance;

    protected bool canAttack;
    protected float timeSinceLastAttack;

    protected bool canWander;
    protected float wanderPauseTime = 3f;
    protected float timeSinceLastWander;
    [SerializeField] protected Vector2 wanderPointA;
    [SerializeField] protected Vector2 wanderPointB;
    protected Vector2 currentWanderPoint;

    protected Transform target;

    protected Core core;

    protected void Awake()
    {
        core = GetComponentInChildren<Core>();
        canAttack = true;
        canWander = true;
    }

    protected void Start()
    {
        currentWanderPoint = wanderPointA;
        Wander();
    }

    protected void Update()
    {
        if (!canAttack)
        {
            CheckIfCanAttack();
            core.Movement.SetVelocityZero();
            return;
        }

        if (target)
        {
            Chase();
            return;
        }
        else
        {
            Wander();
        }

        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectDistance, whatIsPlayer);
        if (playerCollider)
        {
            target = playerCollider.transform;
        }
    }

    public virtual void Wander()
    {
        if (!canWander && Time.time >= timeSinceLastWander + wanderPauseTime)
        {
            canWander = true;
        }
        else if (!canWander)
        {
            return;
        }

        int direction = 1;
        Vector2 distanceFromTarget = currentWanderPoint - (Vector2) transform.position;

        if (distanceFromTarget.x < 0)
        {
            direction = -1;
        }

        core.Movement.SetVelocityX(direction * walkSpeed);
        core.Movement.CheckIfShouldFlip(direction);

        if (Vector2.Distance(transform.position, currentWanderPoint) < 0.1f)
        {
            currentWanderPoint = currentWanderPoint == wanderPointA ? wanderPointB : wanderPointA;
            canWander = false;
            timeSinceLastWander = Time.time;
            core.Movement.SetVelocityZero();
        }
    }

    public virtual void Chase()
    {
        int direction = 1;
        Vector2 distanceFromTarget = target.position - transform.position;

        if (distanceFromTarget.x < 0)
        {
            direction = -1;
        }

        if (Mathf.Abs(distanceFromTarget.x) > 0.1f)
        {
            core.Movement.SetVelocityX(direction * walkSpeed);
            core.Movement.CheckIfShouldFlip(direction);
        }
        else
        {
            core.Movement.SetVelocityZero();
        }

        if (distanceFromTarget.magnitude <= attackRange)
        {
            Attack();
        }
    }

    public virtual void Attack()
    {
        canAttack = false; // Also pauses movement
        timeSinceLastAttack = Time.time;
        Debug.Log("Attacking");

    }

    protected void CheckIfCanAttack()
    {
        if (Time.time >= timeSinceLastAttack + attackSpeed)
        {
            canAttack = true;
        }
    }
}
