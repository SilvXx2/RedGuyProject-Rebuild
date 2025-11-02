using UnityEngine;

public class RewindCommand : ICommand
{
    private readonly Rigidbody2D rb;
    private readonly Transform tr;
    private readonly Vector2 targetPosition;
    private readonly float duration;
    private readonly AnimationCurve curve;
    private readonly MonoBehaviour runner;
    private readonly Mace mace;

    public RewindCommand(Rigidbody2D rb, Transform tr, Vector2 targetPosition, float duration, AnimationCurve curve, MonoBehaviour runner, Mace mace)
    {
        this.rb = rb;
        this.tr = tr;
        this.targetPosition = targetPosition;
        this.duration = Mathf.Max(0.01f, duration);
        this.curve = curve != null ? curve : AnimationCurve.Linear(0, 0, 1, 1);
        this.runner = runner;
        this.mace = mace;
    }

    public void Execute()
    {
        if (runner == null || rb == null || tr == null || mace == null) return;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        mace.State = Mace.MaceState.Rewinding;
        runner.StartCoroutine(Rewind());
    }

    private System.Collections.IEnumerator Rewind()
    {
        Vector2 start = tr.position;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            float eval = curve.Evaluate(k);
            tr.position = Vector2.LerpUnclamped(start, targetPosition, eval);
            yield return null;
        }
        tr.position = targetPosition;
        mace.OnRewindCompleted();
    }
}