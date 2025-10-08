using UnityEngine;

public class RunState : ICharacter
{
    PlayerMovement player;
    float horizontalInput;
    [SerializeField] private float speed = 5f;

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
        horizontalInput = Input.GetAxisRaw("Horizontal");

        bool hasInput = horizontalInput != 0f;
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        
        player.transform.Translate(new Vector3(horizontalInput, 0f, 0f) * (Time.deltaTime * speed), Space.World);

        if (!hasInput)
        {
            player.machine.ChangeState(player.machine.IdleState);
            return;
        }

        if (shiftHeld)                      
        {
            Debug.Log("Move");
            player.machine.ChangeState(player.machine.WalkState);
            return;
        }
    }
}
