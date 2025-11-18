using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class BossController : EnemyController
{
    [Header("Disparo")]
    [SerializeField] private BulletInstance bulletPrefab;
    [SerializeField] private BulletFactory bulletFactory;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float cooldownBetweenSets = 4f;
    private float _lastShotSetTime;
    private Rigidbody2D _rb;
    private Transform _tr;

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
        _tr = transform;
    }

    private void Update()
    {
        if (DetectWallAhead())
        {
            moveDirection *= -1;
            NudgeBack(0.02f);
            FlipToDirection(moveDirection);
        }

        Move(moveDirection, patrolSpeed);

        if (Time.time - _lastShotSetTime > cooldownBetweenSets)
        {
            ShootInFourDirections();
            _lastShotSetTime = Time.time;
        }
    }

    private void ShootInFourDirections()
    {
        if (!bulletPrefab || !firePoint) return;

        ShootInDirection(Vector2.up);
        ShootInDirection(Vector2.down);
        ShootInDirection(Vector2.left);
        ShootInDirection(Vector2.right);
    }

    private void ShootInDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        if (bulletFactory == null)
        {
            Debug.LogWarning("BossController no tiene BulletFactory asignado, no se puede disparar.", this);
            return;
        }

        GameObject bullet = bulletFactory.Create(firePoint.position, dir);
        bullet.transform.rotation = rot;
    }

}