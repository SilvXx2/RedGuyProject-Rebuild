using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mace : MonoBehaviour
{
    public enum MaceState { Idle, Falling, Cooldown, Rewinding }

    [SerializeField] private PlayerDetector playerDetector;

    [Header("Caída")]
    [SerializeField] private float fallGravityScale = 4f;
    [SerializeField] private float fallImpulse = 2f;

    [Header("Rewind")]
    [SerializeField] private float impactCooldown = 1.0f;
    [SerializeField] private float rewindDuration = 0.6f;
    [SerializeField] private AnimationCurve rewindCurve = null;

    [Header("Suelo")]
    [SerializeField] private LayerMask groundMask;

    [Header("Daño")] 
    [SerializeField] private int contactDamage = 1;

    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private bool armed = true;

    public MaceState State { get; set; } = MaceState.Idle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        initialPosition = transform.position;
        if (rewindCurve == null) rewindCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        if (playerDetector == null)
            playerDetector = GetComponent<PlayerDetector>();
    }

    private void Update()
    {
        if (State == MaceState.Idle && armed && playerDetector != null && playerDetector.IsPlayerDetected())
        {
            ExecuteFall();
        }
    }

    private void ExecuteFall()
    {
        ICommand fall = new FallCommand(rb, this, fallGravityScale, fallImpulse);
        fall.Execute();
        armed = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(contactDamage);
        }

        if (State == MaceState.Falling && IsGround(collision.collider))
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f;
            State = MaceState.Cooldown;
            StartCoroutine(CooldownThenRewind());
        }
    }

    private System.Collections.IEnumerator CooldownThenRewind()
    {
        yield return new WaitForSeconds(impactCooldown);
        ICommand rewind = new RewindCommand(rb, transform, initialPosition, rewindDuration, rewindCurve, this, this);
        rewind.Execute();
    }

    public void OnRewindCompleted()
    {
        State = MaceState.Idle;
        armed = true;
    }

    private bool IsGround(Collider2D other)
    {
        return ((1 << other.gameObject.layer) & groundMask.value) != 0;
    }

    private void OnDrawGizmosSelected()
    {
        // El dibujo del área de detección ahora lo maneja PlayerDetector
        // en su propio OnDrawGizmosSelected.
    }

    [ContextMenu("Set Current As Initial")]
    private void SetCurrentAsInitial()
    {
        initialPosition = transform.position;
    }
}