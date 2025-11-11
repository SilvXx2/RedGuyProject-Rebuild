using UnityEngine;

public interface IKnockbackCalculator
{
    Vector2 Compute(Transform self, Transform target, float baseForce, float upwardBoost);
}