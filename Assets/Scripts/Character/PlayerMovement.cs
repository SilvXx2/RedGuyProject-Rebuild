using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IKnockbackable
{
    public CharacterStateMachine machine;
    public Rigidbody2D Rigidbody2D { get; private set; }
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    public float CurrentHorizontalSpeed { get; set; }

    // Control de bloqueo tras knockback
    [SerializeField] private float defaultKnockbackLock = 0.15f;
    private float _movementUnlockTime;
    public bool IsMovementLocked => Time.time < _movementUnlockTime;

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        machine.Initialize();
    }

    void Update()
    {
        machine.UpdateState();
    }

    public bool IsGrounded()
    {
        if (groundCheck == null)
        {
            return Mathf.Abs(Rigidbody2D.linearVelocity.y) < 0.01f;
        }

        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public bool JumpPressed
    {
        get
        {
            if (IsMovementLocked) return false; // bloquear input durante knockback
            bool newInput = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
            bool oldInput = Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump");
            return newInput || oldInput;
        }
    }

    // IKnockbackable
    public void ApplyKnockback(Vector2 impulse, float lockSeconds)
    {
        if (Rigidbody2D == null) return;
        Rigidbody2D.linearVelocity = new Vector2(0f, Rigidbody2D.linearVelocity.y); // limpia la X para que el impulso domine
        Rigidbody2D.AddForce(impulse, ForceMode2D.Impulse);
        _movementUnlockTime = Time.time + Mathf.Max(lockSeconds >= 0 ? lockSeconds : defaultKnockbackLock, 0.05f);
    }

    public void FlipToDirection(float dir)
    {
        if (Mathf.Abs(dir) < 0.01f) return;

        Vector3 scale = transform.localScale;
        float sign = Mathf.Sign(dir);

        if (Mathf.Sign(scale.x) != sign)
        {
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }
}
