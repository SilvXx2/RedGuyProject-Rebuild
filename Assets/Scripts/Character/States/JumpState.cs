using UnityEngine;

public class JumpState : ICharacter
{
	private readonly PlayerMovement player;
	private float horizontalInput;
	private float verticalVelocity;
	private float jumpOriginY;

	private readonly float jumpForce = 12f;
	private readonly float gravity = -9.81f;
	private readonly float airSpeed = 5f;

	public JumpState(PlayerMovement player)
	{
		this.player = player;
	}

	public void Enter()
	{
		jumpOriginY = player.transform.position.y;
		verticalVelocity = jumpForce;
	}

	public void Exit()
	{

	}

	public void Update()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");

		bool hasInput = horizontalInput != 0f;
		bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

		// Horizontal air control
		player.transform.Translate(new Vector3(horizontalInput, 0f, 0f) * (Time.deltaTime * airSpeed), Space.World);

		// Apply simple vertical motion with manual gravity
		verticalVelocity += gravity * Time.deltaTime;
		player.transform.Translate(new Vector3(0f, verticalVelocity * Time.deltaTime, 0f), Space.World);

		if (player.transform.position.y <= jumpOriginY)
		{
			Vector3 groundedPosition = player.transform.position;
			groundedPosition.y = jumpOriginY;
			player.transform.position = groundedPosition;
			verticalVelocity = 0f;

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
}