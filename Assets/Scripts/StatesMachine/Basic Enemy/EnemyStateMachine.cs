using UnityEngine;

[DisallowMultipleComponent]
public class EnemyStateMachine : MonoBehaviour
{
    public IEnemyState Current { get; private set; }

    public PatrolState PatrolState { get; private set; }
    public PursueState PursueState { get; private set; }

    [SerializeField] private EnemyController enemy;

    private void Awake()
    {
        if (!enemy) enemy = GetComponent<EnemyController>();
        if (!enemy)
        {
            Debug.LogError("EnemyStateMachine requiere EnemyController.");
            enabled = false;
            return;
        }

        PatrolState = new PatrolState(enemy, this);
        PursueState = new PursueState(enemy, this);

        ChangeState(PatrolState);
    }

    private void Update()
    {
        Current?.Update();
    }

    public void ChangeState(IEnemyState next)
    {
        if (next == null || next == Current) return;
        Current?.Exit();
        Current = next;
        Current.Enter();
    }
}