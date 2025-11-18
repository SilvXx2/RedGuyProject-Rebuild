using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/Health Pickup Type", fileName = "HealthType")]
public class HealthType : ScriptableObject
{
    [Header("Vida")]
    public int healAmount = 1;

    [Header("Comportamiento")]
    public bool destroyOnPickup = true;
}
