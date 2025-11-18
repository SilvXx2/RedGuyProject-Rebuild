using UnityEngine;

public class PlayerShootConfig : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private BulletFactory bulletFactory;

    private void Update()
    {
        if (player != null && bulletFactory != null)
        {
            bulletFactory.PlayerBulletSpeedMultiplier = player.BulletSpeedMultiplier;
        }
    }
}