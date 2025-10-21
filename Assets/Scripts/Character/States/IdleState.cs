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
        horizontalInput = Input.GetAxisRaw("Horizontal");
        player.CurrentHorizontalSpeed = 0f;

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

        Shoot();
    }

    public void Shoot() 
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            player.machine.ShootState.SetReturn(this);
            player.machine.ChangeState(player.machine.ShootState);
            return;
        }
    }
}
