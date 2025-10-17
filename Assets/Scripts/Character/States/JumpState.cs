using UnityEngine;

public class JumpState : ICharacter
{
    private readonly PlayerMovement player;
    private float horizontalInput;

    private readonly float jumpForce = 12f;
    private readonly float airSpeed = 7.5f;
    private readonly float airAcceleration = 16f;
    private readonly float reverseControlMultiplier = 2.25f;
    private readonly float gravity = -30f;

    public JumpState(PlayerMovement player)
    {
        this.player = player;
    }

    public void Enter()
    {
        float preservedX = Mathf.Approximately(player.CurrentHorizontalSpeed, 0f)
            ? Input.GetAxisRaw("Horizontal") * airSpeed
            : player.CurrentHorizontalSpeed;

        Vector2 velocity = player.Rigidbody2D.linearVelocity;
        velocity.x = preservedX;
        velocity.y = 0f;
        player.Rigidbody2D.linearVelocity = velocity;

        player.Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Exit()
    {

    }

    public void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        bool hasInput = !Mathf.Approximately(horizontalInput, 0f);
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        float baseSpeed = Mathf.Max(Mathf.Abs(player.CurrentHorizontalSpeed), airSpeed);
        float desiredSpeed = horizontalInput * airSpeed * (shiftHeld ? 1.4f : 1f);

        Vector2 velocity = player.Rigidbody2D.linearVelocity;

        bool reversing = hasInput &&
                         Mathf.Sign(desiredSpeed) != Mathf.Sign(velocity.x) &&
                         Mathf.Abs(velocity.x) > 0.1f;

        float acceleration = reversing ? airAcceleration * reverseControlMultiplier : airAcceleration;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredSpeed, acceleration * Time.deltaTime);
        player.Rigidbody2D.linearVelocity = velocity;
        player.CurrentHorizontalSpeed = velocity.x;

        if (!player.IsGrounded())
        {
            return;
        }

        if (!hasInput)
        {
            player.CurrentHorizontalSpeed = 0f;
            player.machine.ChangeState(player.machine.IdleState);
            return;
        }

        if (shiftHeld)
        {
            player.machine.ChangeState(player.machine.RunState);
            return;
        }

        player.machine.ChangeState(player.machine.WalkState);
    }
}