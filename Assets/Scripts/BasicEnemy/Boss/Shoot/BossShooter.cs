using UnityEngine;

public class BossShooter : MonoBehaviour
{
    [SerializeField] private BulletFactory bulletFactory;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float cooldownBetweenSets = 4f;

    private float _lastShotSetTime;

    private void Awake()
    {
        if (bulletFactory == null)
            bulletFactory = GetComponent<BulletFactory>();
    }

    public void Tick(float time)
    {
        if (time - _lastShotSetTime <= cooldownBetweenSets)
            return;

        ShootInFourDirections();
        _lastShotSetTime = time;
    }

    private void ShootInFourDirections()
    {
        if (bulletFactory == null || firePoint == null)
            return;

        ShootInDirection(Vector2.up);
        ShootInDirection(Vector2.down);
        ShootInDirection(Vector2.left);
        ShootInDirection(Vector2.right);
    }

    private void ShootInDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject bullet = bulletFactory.Create(firePoint.position, dir);
        if (bullet != null)
            bullet.transform.rotation = rot;
    }
}
