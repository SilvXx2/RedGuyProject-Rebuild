using UnityEngine;

public class FallCommand : ICommand
{
    private readonly Rigidbody2D rb;
    private readonly Mace mace;
    private readonly float gravityScale;
    private readonly float downwardImpulse;

    public FallCommand(Rigidbody2D rb, Mace mace, float gravityScale, float downwardImpulse)
    {
        this.rb = rb;
        this.mace = mace;
        this.gravityScale = gravityScale;
        this.downwardImpulse = downwardImpulse;
    }

    public void Execute()
    {
        if (rb == null || mace == null) return;
        mace.State = Mace.MaceState.Falling;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = gravityScale;
        if (downwardImpulse > 0f)
            rb.AddForce(Vector2.down * downwardImpulse, ForceMode2D.Impulse);
    }
}