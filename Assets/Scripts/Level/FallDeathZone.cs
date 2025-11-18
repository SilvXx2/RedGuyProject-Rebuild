using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FallDeathZone : MonoBehaviour
{
    [SerializeField] private int lethalDamage = 9999;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(lethalDamage);
        }
        else
        {
            Debug.LogWarning("FallDeathZone: el Player no implementa IDamageable. Considera añadir Health/IDamageable al jugador.");
        }
    }
}