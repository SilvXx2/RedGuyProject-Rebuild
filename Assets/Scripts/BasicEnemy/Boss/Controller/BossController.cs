using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class BossController : EnemyController
{
    [SerializeField] private BossShooter shooter;

    private Rigidbody2D _rb;
    private Transform _tr;

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
        _tr = transform;

        if (shooter == null)
            shooter = GetComponent<BossShooter>();
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

        if (shooter != null)
            shooter.Tick(Time.time);
    }

}