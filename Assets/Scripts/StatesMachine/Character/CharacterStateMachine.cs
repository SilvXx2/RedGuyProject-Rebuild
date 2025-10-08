using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterStateMachine : MonoBehaviour
{
    ICharacter actualState;

    private IdleState _idleState;
    private WalkState _walkState;
    private RunState _runState;

    // Propiedades con nombres consistentes (PascalCase)
    public IdleState IdleState => _idleState;
    public WalkState WalkState => _walkState;
    public RunState RunState => _runState;

    PlayerMovement player;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        _idleState = new IdleState(player);
        _walkState = new WalkState(player);
        _runState = new RunState(player);
    }

    public void Initialize()
    {
        actualState = _idleState;  // Usa la variable privada
        actualState.Enter();
    }

    public void UpdateState()
    {
        actualState.Update();
    }

    public void ChangeState(ICharacter newState)
    {
        actualState.Exit();
        actualState = newState;
        actualState.Enter();
    }
}
