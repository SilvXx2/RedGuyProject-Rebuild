using UnityEngine;

// Estrategia: empuja al objetivo en dirección opuesta a su desplazamiento horizontal.
// Si no hay desplazamiento claro (velocidad ~0), lo empuja lejos del objeto (según posición relativa).
public class OppositeVelocityKnockback : IKnockbackCalculator
{
    public Vector2 Compute(Transform self, Transform target, float baseForce, float upwardBoost)
    {
        if (self == null || target == null) return Vector2.zero;

        // 1) Intentar con el controlador del jugador (más fiable si se mueve con Translate)
        float moveX = 0f;
        var pm = target.GetComponent<PlayerMovement>();
        if (pm != null)
            moveX = pm.CurrentHorizontalSpeed;
        else
        {
            // 2) Fallback a física si existe
            var rb = target.GetComponent<Rigidbody2D>();
            if (rb != null) moveX = rb.linearVelocity.x;
        }

        const float threshold = 0.05f;
        float horizontalDir;
        if (Mathf.Abs(moveX) > threshold)
        {
            // Opuesto al movimiento actual
            horizontalDir = -Mathf.Sign(moveX);
        }
        else
        {
            // Quieto o casi: empujar lejos del objeto según posiciones
            horizontalDir = (target.position.x >= self.position.x) ? 1f : -1f;
        }

        Vector2 dir = new Vector2(horizontalDir, upwardBoost);
        dir = dir.normalized * baseForce;
        return dir;
    }
}
