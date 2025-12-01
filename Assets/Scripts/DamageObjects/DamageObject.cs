using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public float explosionForce = 1f;
    public float upwardBoost = 0.2f;
    public LayerMask playerMask;
    [SerializeField, Min(0f)] private float hitCooldown = 0.1f; 
    [SerializeField, Min(0f)] private float inputLockSeconds = 0.15f; 
    [Header("Top-collision tuning")]
    [SerializeField, Min(1f)] private float topHitForceMultiplier = 1.5f; 
    [SerializeField, Min(0f)] private float topExtraUpward = 0.4f; 

    private float _nextHitTime;
    private bool _isTouching;

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
        if (!_isTouching) return;
        var rb = collision.rigidbody;
        if (rb == null) return;


        var force = calc.Compute(transform, collision.transform, explosionForce, upwardBoost);

        var contacts = new ContactPoint2D[1];
        int count = collision.GetContacts(contacts);
        if (count > 0)
        {
            Vector2 n = contacts[0].normal;
            if (n.y > 0.5f)
            {
                force += Vector2.up * topExtraUpward;
                force *= topHitForceMultiplier;
            }
        }

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