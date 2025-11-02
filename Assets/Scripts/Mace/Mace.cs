using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mace : MonoBehaviour
{
    public enum MaceState { Idle, Falling, Cooldown, Rewinding }

    [Header("Detección jugador")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float detectWidth = 1.0f;
    [SerializeField] private float detectDistance = 5f;
    [SerializeField] private Vector2 detectOffset;

    [Header("Caída")]
    [SerializeField] private float fallGravityScale = 4f;
    [SerializeField] private float fallImpulse = 2f;

    [Header("Rewind")]
    [SerializeField] private float impactCooldown = 1.0f;
    [SerializeField] private float rewindDuration = 0.6f;
    [SerializeField] private AnimationCurve rewindCurve = null;

    [Header("Suelo")]
    [SerializeField] private LayerMask groundMask;

    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private bool armed = true;

    public MaceState State { get; set; } = MaceState.Idle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // importante para evitar atravesar
        initialPosition = transform.position;
        if (rewindCurve == null) rewindCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }

    private void Update()
    {
        if (State == MaceState.Idle && armed && DetectPlayerBelow())
        {
            ExecuteFall();
        }
    }

    private bool DetectPlayerBelow()
    {
        Vector2 center = (Vector2)transform.position + detectOffset + Vector2.down * (detectDistance * 0.5f);
        Vector2 size = new Vector2(detectWidth, detectDistance);
        Collider2D hit = Physics2D.OverlapBox(center, size, 0f, playerMask);
        return hit != null;
    }

    private void ExecuteFall()
    {
        ICommand fall = new FallCommand(rb, this, fallGravityScale, fallImpulse);
        fall.Execute();
        armed = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (State == MaceState.Falling && IsGround(collision.collider))
        {
            // Enfriar tras impacto y luego volver
            rb.velocity = Vector2.zero;          // CORREGIDO
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
        // Más robusto: compara contra el LayerMask del “ground”
        return ((1 << other.gameObject.layer) & groundMask.value) != 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + (Vector3)detectOffset + Vector3.down * (detectDistance * 0.5f);
        Vector3 size = new Vector3(detectWidth, detectDistance, 0.1f);
        Gizmos.DrawWireCube(center, size);
    }

    [ContextMenu("Set Current As Initial")]
    private void SetCurrentAsInitial()
    {
        initialPosition = transform.position;
    }
}