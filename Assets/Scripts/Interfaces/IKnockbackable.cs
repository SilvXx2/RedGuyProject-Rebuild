using UnityEngine;

public interface IKnockbackable
{
    void ApplyKnockback(Vector2 impulse, float lockSeconds);
    bool IsMovementLocked { get; }
}
