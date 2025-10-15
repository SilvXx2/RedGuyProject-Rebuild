using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterStateMachine machine;
    public Rigidbody2D Rigidbody2D { get; private set; }
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

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
}
