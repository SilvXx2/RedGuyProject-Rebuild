using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterStateMachine : MonoBehaviour
{
    ICharacter actualState;

    private IdleState _idleState;
    private WalkState _walkState;
    private RunState _runState;
    private JumpState _jumpState;
    private ShootState _shootState;

    // Propiedades con nombres consistentes (PascalCase)
    public IdleState IdleState => _idleState;
    public WalkState WalkState => _walkState;
    public RunState RunState => _runState;
    public JumpState JumpState => _jumpState;
    public ShootState ShootState => _shootState;

    [SerializeField] private PlayerMovement player;

    // Nuevo: referencias para disparo sin Shooter
    [SerializeField] private BulletFactory bulletFactory;
    [SerializeField] private Transform firePoint;

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

        // Resolver referencias para el disparo
        if (bulletFactory == null)
            bulletFactory = GetComponent<BulletFactory>() ?? player.GetComponent<BulletFactory>();

        if (firePoint == null)
            firePoint = player.transform;

        if (bulletFactory == null)
            Debug.LogWarning("No se encontró BulletFactory. Agrega y configura BulletFactory en el Player.");

        _shootState = new ShootState(this, bulletFactory, firePoint);
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
