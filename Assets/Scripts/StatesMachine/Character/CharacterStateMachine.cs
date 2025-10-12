using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterStateMachine : MonoBehaviour
{
    ICharacter actualState;

    private IdleState _idleState;
    private WalkState _walkState;
    private RunState _runState;
    private JumpState _jumpState;

    // Propiedades con nombres consistentes (PascalCase)
    public IdleState IdleState => _idleState;
    public WalkState WalkState => _walkState;
    public RunState RunState => _runState;
    public JumpState JumpState => _jumpState;

    [SerializeField] private PlayerMovement player;

    private void Awake()
    {
        if (player == null)
        {
            player = GetComponent<PlayerMovement>();
        }

        if (player == null)
        {
            player = GetComponentInParent<PlayerMovement>();
        }

        if (player == null)
        {
            Debug.LogError("CharacterStateMachine requiere una referencia a PlayerMovement en el mismo objeto o en sus padres.", this);
            enabled = false;
            return;
        }

        if (player != null)
        {
            player.machine = this; // ensure states can access the state machine instance
        }
        _idleState = new IdleState(player);
        _walkState = new WalkState(player);
        _runState = new RunState(player);
        _jumpState = new JumpState(player);
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
