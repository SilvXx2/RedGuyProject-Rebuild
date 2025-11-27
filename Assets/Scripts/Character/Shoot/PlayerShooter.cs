using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private BulletFactory bulletFactory;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.75f;

    private PlayerMovement _player;
    private float _nextShootTime;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();

        if (bulletFactory == null)
            bulletFactory = GetComponent<BulletFactory>();

        if (firePoint == null)
            firePoint = transform;
    }

    public bool CanShoot => Time.time >= _nextShootTime && bulletFactory != null && firePoint != null;

    public void Shoot()
    {
        if (!CanShoot) return;

        float facing = Mathf.Sign(firePoint.root.localScale.x);
        Vector3 dir = facing > 0 ? Vector3.right : Vector3.left;

        bulletFactory.PlayerBulletSpeedMultiplier = _player != null ? _player.BulletSpeedMultiplier : 1f;
        bulletFactory.Create(firePoint.position, dir);

        _nextShootTime = Time.time + shootCooldown;
    }

    public void SetCooldown(float seconds)
    {
        shootCooldown = Mathf.Max(0f, seconds);
    }
}
