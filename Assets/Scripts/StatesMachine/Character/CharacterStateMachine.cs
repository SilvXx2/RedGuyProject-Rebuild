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

    public IdleState IdleState => _idleState;
    public WalkState WalkState => _walkState;
    public RunState RunState => _runState;
    public JumpState JumpState => _jumpState;
    public ShootState ShootState => _shootState;

    [SerializeField] private PlayerMovement player;

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
            Debug.LogError("no hay playermovement", this);
            enabled = false;
            return;
        }

        if (player != null)
        {
            player.machine = this; 
        }
        _idleState = new IdleState(player);
        _walkState = new WalkState(player);
        _runState = new RunState(player);
        _jumpState = new JumpState(player);

        if (bulletFactory == null)
            bulletFactory = GetComponent<BulletFactory>() ?? player.GetComponent<BulletFactory>();

        if (firePoint == null)
            firePoint = player.transform;

        if (bulletFactory == null)
            Debug.LogWarning("no hay bulltefactory");

        _shootState = new ShootState(this, bulletFactory, firePoint);
    }

    public void Initialize()
    {
        actualState = _idleState; 
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
