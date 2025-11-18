using UnityEngine;

public class ShootState : ICharacter
{
    private readonly CharacterStateMachine _machine;
    private readonly BulletFactory _factory;
    private readonly Transform _firePoint;

    private ICharacter _returnState;
    private bool _shot;

    private float _cooldown = 0.75f;
    private float _nextShootTime;

    public ShootState(CharacterStateMachine machine, BulletFactory factory, Transform firePoint)
    {
        _machine = machine;
        _factory = factory;
        _firePoint = firePoint;
    }

    public void SetReturn(ICharacter stateToReturn)
    {
        _returnState = stateToReturn;
    }

    public void SetCooldown(float seconds)
    {
        _cooldown = Mathf.Max(0f, seconds);
    }

    public bool CanShoot => Time.time >= _nextShootTime;

    public void Enter()
    {
        _shot = false;
    }

    public void Exit() { }

    public void Update()
    {
        if (_shot) return;

        if (CanShoot && _factory != null && _firePoint != null)
        {
            float facing = Mathf.Sign(_firePoint.root.localScale.x);
            Vector3 dir = facing > 0 ? Vector3.right : Vector3.left;

            _factory.Create(_firePoint.position, dir);
            _nextShootTime = Time.time + _cooldown;
        }

        _shot = true;
        _machine.ChangeState(_returnState ?? _machine.IdleState);
    }
}