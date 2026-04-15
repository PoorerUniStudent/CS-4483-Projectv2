using UnityEngine;

public class RangedEnemy : Enemy
{
    public GameObject proj;
    public override void OnAttackTrigger()
    {
        GameObject projClone = Instantiate(proj, transform.position, Quaternion.identity);
        projClone.GetComponent<Projectile>().ChangeDirection(core.Movement.facingDir);
    }
}
