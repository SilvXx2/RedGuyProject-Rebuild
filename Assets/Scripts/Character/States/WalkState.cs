using UnityEngine;
using UnityEngine.InputSystem;


public class WalkState : ICharacter
{
    PlayerMovement player;
    float horizontalInput;
    private float speed = 5f;

    public WalkState(PlayerMovement player)
    {
        this.player = player;
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void Update()
    {
        // Si está bloqueado por knockback, ignorar input y no mover
        if (player is IKnockbackable kb && kb.IsMovementLocked)
        {
            player.CurrentHorizontalSpeed = 0f;
            return;
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");

        bool hasInput = horizontalInput != 0f;
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    bool jumpPressed = !((player is IKnockbackable kbl) && kbl.IsMovementLocked) && Input.GetButtonDown("Jump");

        if (jumpPressed)
        {
            player.machine.ChangeState(player.machine.JumpState);
            return;
        }

        
        player.transform.Translate(new Vector3(horizontalInput, 0f, 0f) * (Time.deltaTime * speed), Space.World);
        player.CurrentHorizontalSpeed = horizontalInput * speed;

        if (!hasInput)
        {
            player.CurrentHorizontalSpeed = 0f;
            player.machine.ChangeState(player.machine.IdleState);
            return;
        }

        if (shiftHeld)
        {
            Debug.Log("Run");
            player.machine.ChangeState(player.machine.RunState);
            return;
        }
    }
}
