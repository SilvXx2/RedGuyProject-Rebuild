using UnityEngine;

public class PursueState : IEnemyState
{
    private readonly EnemyController enemy;
    private readonly EnemyStateMachine machine;

    // Opcional: tiempo que puede perder de vista antes de volver a patrulla
    private readonly float loseSightGrace = 0.3f;
    private float loseTimer;

    public PursueState(EnemyController enemy, EnemyStateMachine machine)
    {
        this.enemy = enemy;
        this.machine = machine;
    }

    public void Enter()
    {
        loseTimer = 0f;
    }

    public void Exit() { }

    public void Update()
    {
        // Si no ve al player, contar gracia y volver a patrulla
        if (!enemy.CanSeePlayer())
        {
            loseTimer += Time.deltaTime;
            if (loseTimer >= loseSightGrace)
            {
                machine.ChangeState(machine.PatrolState);
                return;
            }
        }
        else
        {
            loseTimer = 0f;
        }

        // Dirigirse hacia el jugador
        int dirToPlayer = enemy.DirectionToPlayer();
        enemy.FlipToDirection(dirToPlayer);

        // Evitar atravesar pared: si hay pared delante, no avanzar
        if (!enemy.DetectWallAhead())
        {
            enemy.Move(dirToPlayer, enemy.pursueSpeed);
        }
    }
}
