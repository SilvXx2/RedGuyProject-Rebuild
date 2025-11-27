using UnityEngine;

public interface IPlayerDetector
{
    bool IsPlayerDetected();
}

public class PlayerDetector : MonoBehaviour, IPlayerDetector
{
    [Header("Detección jugador")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float detectWidth = 1.0f;
    [SerializeField] private float detectDistance = 5f;
    [SerializeField] private Vector2 detectOffset;

    public bool IsPlayerDetected()
    {
        Vector2 center = (Vector2)transform.position + detectOffset + Vector2.down * (detectDistance * 0.5f);
        Vector2 size = new Vector2(detectWidth, detectDistance);
        Collider2D hit = Physics2D.OverlapBox(center, size, 0f, playerMask);
        return hit != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + (Vector3)detectOffset + Vector3.down * (detectDistance * 0.5f);
        Vector3 size = new Vector3(detectWidth, detectDistance, 0.1f);
        Gizmos.DrawWireCube(center, size);
    }
}
