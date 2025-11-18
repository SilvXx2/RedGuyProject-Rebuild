using UnityEngine;

public class OppositeVelocityKnockback : IKnockbackCalculator
{
    public Vector2 Compute(Transform self, Transform target, float baseForce, float upwardBoost)
    {
        if (self == null || target == null) return Vector2.zero;

        float moveX = 0f;
        var pm = target.GetComponent<PlayerMovement>();
        if (pm != null)
            moveX = pm.CurrentHorizontalSpeed;
        else
        {
            var rb = target.GetComponent<Rigidbody2D>();
            if (rb != null) moveX = rb.linearVelocity.x;
        }

        const float threshold = 0.05f;
        float horizontalDir;
        if (Mathf.Abs(moveX) > threshold)
        {
            horizontalDir = -Mathf.Sign(moveX);
        }
        else
        {
            horizontalDir = (target.position.x >= self.position.x) ? 1f : -1f;
        }

        Vector2 dir = new Vector2(horizontalDir, upwardBoost);
        dir = dir.normalized * baseForce;
        return dir;
    }
}
