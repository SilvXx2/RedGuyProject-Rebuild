using UnityEngine;

public class JumpState : ICharacter
{
    private readonly PlayerMovement player;
    private float horizontalInput;
    private float verticalVelocity;
    private float jumpOriginY;

    private readonly float jumpForce = 12f;
    private readonly float airSpeed = 5f;

    public JumpState(PlayerMovement player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Rigidbody2D.linearVelocity = new Vector2(player.Rigidbody2D.linearVelocity.x, 0f);
        player.Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Exit()
    {

    }

    public void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        bool hasInput = horizontalInput != 0f;
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        Vector2 velocity = player.Rigidbody2D.linearVelocity;
        velocity.x = horizontalInput * airSpeed;
        player.Rigidbody2D.linearVelocity = velocity;

        if (!player.IsGrounded())
        {
            return;
        }

        if (!hasInput)
        {
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