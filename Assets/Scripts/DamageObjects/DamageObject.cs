using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public float explosionForce = 1f;
    public float upwardBoost = 0.2f;
    public LayerMask playerMask;
    [SerializeField, Min(0f)] private float hitCooldown = 0.1f; // evita aplicar fuerza muchos frames seguidos
    [SerializeField, Min(0f)] private float inputLockSeconds = 0.15f; // tiempo de inhabilitar input tras el golpe
    [Header("Top-collision tuning")]
    [SerializeField, Min(1f)] private float topHitForceMultiplier = 1.5f; // multiplica la fuerza si el contacto es por arriba
    [SerializeField, Min(0f)] private float topExtraUpward = 0.4f; // impulso vertical extra al caer encima

    private float _nextHitTime;
    private bool _isTouching;

    // Usamos estrategia que empuja contra la dirección de movimiento del jugador
    private IKnockbackCalculator calc = new OppositeVelocityKnockback();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isTouching = true;
        TryApplyKnockback(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((playerMask.value & (1 << collision.gameObject.layer)) == 0) return;
        _isTouching = false;
    }

    private void TryApplyKnockback(Collision2D collision)
    {
        if (Time.time < _nextHitTime) return;
        if ((playerMask.value & (1 << collision.gameObject.layer)) == 0) return;
        if (!_isTouching) return; // Solo aplicar si la colisión acaba de comenzar o sigue tocando desde Enter

        var rb = collision.rigidbody;
        if (rb == null) return;

        // No forzamos a 0 la velocidad X; dejamos que el impulso actúe sin "pegar" al jugador

        var force = calc.Compute(transform, collision.transform, explosionForce, upwardBoost);

        // Si el contacto es principalmente desde arriba (normal del bloque hacia +Y), refuerza el impulso
        var contacts = new ContactPoint2D[1];
        int count = collision.GetContacts(contacts);
        if (count > 0)
        {
            Vector2 n = contacts[0].normal; // normal relativa a este objeto (DamageObject)
            if (n.y > 0.5f)
            {
                force += Vector2.up * topExtraUpward; // más componente vertical
                force *= topHitForceMultiplier;        // subir magnitud total
            }
        }
        // Si el objeto golpeado implementa IKnockbackable, usar su método para bloquear movimiento
        var kb = collision.gameObject.GetComponent<IKnockbackable>();
        if (kb != null)
        {
            kb.ApplyKnockback(force, inputLockSeconds);
        }
        else
        {
            rb.AddForce(force, ForceMode2D.Impulse);
        }

        _nextHitTime = Time.time + hitCooldown;
    }
}