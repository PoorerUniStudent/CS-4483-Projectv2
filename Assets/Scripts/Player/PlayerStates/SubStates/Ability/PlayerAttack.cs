using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAttack : PlayerAbility
{
	private Transform target;
	private float enemyHitRadius = 0.3f;
	private LayerMask whatIsEnemy;
	
	private float attackCooldown = 0.4f;
	private float attackPauseTime = 0.2f;
	private float timeSinceLastAttack;
	private float timeSinceAttackStart;

	const float maxAttackTime = 2f;

	public bool canAttack { get; private set; }
	private bool canDisableCollision;

	public PlayerAttack(Player player, PlayerFiniteStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
		canAttack = true;
    }

	public override void Enter()
	{
		base.Enter();
        
        whatIsEnemy = core.CollisionSenses.WhatIsEnemy;
		
		if (target == null || !canAttack)
		{
			isAbilityDone = true;
			return;
		}

        canAttack = false;
        timeSinceAttackStart = Time.time;
		canDisableCollision = true;
    }

	public override void Exit()
	{
		base.Exit();

        float momentumCarry = 0.2f;
        Vector2 currentVel = core.Movement.rb.linearVelocity;
        core.Movement.SetVelocity(currentVel.magnitude * momentumCarry, currentVel.normalized);

        timeSinceLastAttack = Time.time;
		canAttack = true;
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (Time.time < timeSinceAttackStart + attackPauseTime)
		{
			core.Movement.SetVelocityZero();
			return;
        }

			// Pause all actins the enemy is doing target.FreezeActions();
			int direction = 1;
        Vector2 distanceFromTarget = target.position - player.transform.position;

        if (distanceFromTarget.x < 0)
        {
            direction = -1;
        }

        core.Movement.CheckIfShouldFlip(direction);
        core.Movement.SetVelocity(charData.dashForce, distanceFromTarget);

        if (Time.time >= timeSinceAttackStart + maxAttackTime)
		{
			isAbilityDone = true;
		}
		
		Collider2D enemy = Physics2D.OverlapCircle(player.transform.position, enemyHitRadius, whatIsEnemy);
		
		if (enemy != null && enemy.transform == target)
		{
			isAbilityDone = true;
			// Unfreeze enemy
			enemy.GetComponent<Enemy>().Die();
		}
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public bool CheckIfCanAttack()
	{
		return canAttack && timeSinceLastAttack + attackCooldown <= Time.time;
	}
}
