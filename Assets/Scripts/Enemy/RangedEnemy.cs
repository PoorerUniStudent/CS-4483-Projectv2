using UnityEngine;

public class RangedEnemy : Enemy
{
    public GameObject proj;
    private bool isAttacking;
    public override void Chase()
    {
        if (isAttacking)
        {
            return;
        }

        base.Chase();
    }
    public override void Attack()
    {
        isAttacking = true;
        base.Attack();
    }
    public override void OnAttackTrigger()
    {
        Vector2 offset = new Vector2(0, 0.15f);
        GameObject projClone = Instantiate(proj, (Vector2) transform.position + offset, Quaternion.identity);

        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projClone.GetComponent<Projectile>().ChangeDirection(direction, angle);
    }

    public override void OnAttackFinished()
    {
        base.OnAttackFinished();
        isAttacking = false;
    }
}
