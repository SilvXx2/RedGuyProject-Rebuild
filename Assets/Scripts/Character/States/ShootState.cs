using UnityEngine;

public class ShootState : ICharacter
{
    private readonly CharacterStateMachine _machine;
    private readonly PlayerShooter _shooter;

    private ICharacter _returnState;
    private bool _shot;

    public ShootState(CharacterStateMachine machine, BulletFactory factory, Transform firePoint)
    {
        _machine = machine;
        _shooter = machine != null ? machine.PlayerShooter : null;
    }

    public void SetReturn(ICharacter stateToReturn)
    {
        _returnState = stateToReturn;
    }

    public void SetCooldown(float seconds)
    {
        if (_shooter != null)
            _shooter.SetCooldown(seconds);
    }

    public void Enter()
    {
        _shot = false;
    }

    public void Exit() { }

    public void Update()
    {
        if (_shot) return;

        if (_shooter != null && _shooter.CanShoot)
        {
            _shooter.Shoot();
        }

        _shot = true;
        _machine.ChangeState(_returnState ?? _machine.IdleState);
    }
}