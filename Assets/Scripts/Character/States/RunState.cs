using UnityEngine;

public class RunState : ICharacter
{
    PlayerMovement player;
    float horizontalInput;
    [SerializeField] private float baseSpeed = 10f;

    public RunState(PlayerMovement player)
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

        float finalSpeed = baseSpeed * player.SpeedMultiplier;

        player.transform.Translate(new Vector3(horizontalInput, 0f, 0f) * (Time.deltaTime * finalSpeed), Space.World);
        player.CurrentHorizontalSpeed = horizontalInput * finalSpeed;
        player.FlipToDirection(horizontalInput);
        if (player.Animator)
        {
            player.Animator.SetBool("isRun", true);
            player.Animator.SetBool("isJump", false);
        }

        if (!hasInput)
        {
            player.CurrentHorizontalSpeed = 0f;
            player.machine.ChangeState(player.machine.IdleState);
            return;
        }

        if (!shiftHeld)
        {
            player.machine.ChangeState(player.machine.WalkState);
            return;
        }
    }
}
