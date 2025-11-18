using UnityEngine;

public class HealthPick : MonoBehaviour
{
    [SerializeField] private HealthType healthType;

    private void Awake()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (healthType == null)
        {
            Debug.LogWarning("HealthPickup sin HealthType asignado.", this);
            return;
        }

        var health = other.GetComponent<Health>();
        if (health == null || health.IsDead) return;

        health.Heal(healthType.healAmount);

        if (healthType.destroyOnPickup)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}