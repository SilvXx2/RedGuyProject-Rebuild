using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private LayerMask damageMask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((damageMask.value & (1 << collision.gameObject.layer)) == 0)
            return;

        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(contactDamage);
        }
    }
}
