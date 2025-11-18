using UnityEngine;

public class PatrolState : IEnemyState
{
    private readonly EnemyController enemy;
    private readonly EnemyStateMachine machine;

    public PatrolState(EnemyController enemy, EnemyStateMachine machine)
    {
        this.enemy = enemy;
        this.machine = machine;
    }

    public void Enter() { }
    
    public void Exit() { }

    public void Update()
    {
        if (enemy.CanSeePlayer())
        {
            machine.ChangeState(machine.PursueState);
            return;
        }

        if (enemy.DetectWallAhead())
        {
            enemy.moveDirection *= -1;
            enemy.NudgeBack(0.02f);
            enemy.FlipToDirection(enemy.moveDirection);
        }

        enemy.Move(enemy.moveDirection, enemy.patrolSpeed);
    }
}