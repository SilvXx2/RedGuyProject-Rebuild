using UnityEngine;

[DisallowMultipleComponent]
public class PlayerDamageReceiver : MonoBehaviour, IDamageable
{
    [SerializeField] private Health health;

    private void Awake()
    {
        if (!health) 
        {
            health = GetComponent<Health>();
        }
    }

    public void TakeDamage(int amount)
    {
        if (health != null)
        {
            health.TakeDamage(amount);
        }
    }
}