using UnityEngine;

public class RadialKnockback : IKnockbackCalculator
{
    public Vector2 Compute(Transform self, Transform target, float baseForce, float upwardBoost)
    {
        if (self == null || target == null)
            return Vector2.zero;

        Vector2 dir = ((Vector2)target.position - (Vector2)self.position).normalized;
        dir += Vector2.up * upwardBoost;
        return dir.normalized * baseForce;
    }
}
