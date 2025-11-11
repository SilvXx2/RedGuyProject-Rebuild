using UnityEngine;

public interface IKnockbackable
{
    // Aplica un impulso externo y bloquea el movimiento del controlador por 'lockSeconds'
    void ApplyKnockback(Vector2 impulse, float lockSeconds);
    bool IsMovementLocked { get; }
}
