using UnityEngine;
using UnityEngine.InputSystem;


public class IdleState : ICharacter
{
    PlayerMovement player;
    float horizontalInput;

    public IdleState(PlayerMovement player)
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
        // Movimiento 2D: solo eje X
        horizontalInput = Input.GetAxisRaw("Horizontal");

        bool hasInput = horizontalInput != 0f;
        bool jumpPressed = Input.GetButtonDown("Jump");

        if (jumpPressed)
        {
            player.machine.ChangeState(player.machine.JumpState);
            return;
        }

        if (hasInput)
        {
            player.machine.ChangeState(player.machine.WalkState);
        }
    }
}
