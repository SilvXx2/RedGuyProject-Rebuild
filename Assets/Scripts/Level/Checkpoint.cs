using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [Tooltip("Posición de respawn.")]
    public Transform respawnPoint;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        gameObject.tag = "Untagged";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (CheckpointManager.Instance == null)
        {
            Debug.LogWarning("No existe CheckpointManager en la escena.");
            return;
        }

        Vector3 pos = respawnPoint != null ? respawnPoint.position : transform.position;
        CheckpointManager.Instance.SaveCheckpoint(pos);
    }
}
