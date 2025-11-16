using UnityEngine;

public class PlayerBulletInstance : BulletInstance
{
    [SerializeField] private int damage = 1;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerMovement>(out var player))
        {
            Pool?.ReturnToPool(this);
            return;
        }

        if ((destroyOnLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            Pool?.ReturnToPool(this);
        }
    }
}
