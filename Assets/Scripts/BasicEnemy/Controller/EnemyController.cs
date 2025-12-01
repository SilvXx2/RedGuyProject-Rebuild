using UnityEngine;

[DisallowMultipleComponent]
public class EnemyController : MonoBehaviour
{
    [Header("Tipo de enemigo (Type Object)")]
    [SerializeField] private EnemyType type;
    public EnemyType Type => type;

    [HideInInspector] public float patrolSpeed;
    [HideInInspector] public float pursueSpeed;
    [HideInInspector] public int moveDirection;
    [HideInInspector] public LayerMask wallMask;
    [HideInInspector] public float wallRayDistance;
    [HideInInspector] public LayerMask playerMask;
    [HideInInspector] public float viewDistance;

    [SerializeField] private Transform player;

    private Transform tr;
    private int currentHealth;

    protected virtual void Awake()
    {
        tr = transform;

        if (type != null)
        {
            patrolSpeed     = type.patrolSpeed;
            pursueSpeed     = type.pursueSpeed;
            moveDirection   = type.initialDirection == 0 ? 1 : type.initialDirection;
            wallMask        = type.wallMask;
            wallRayDistance = type.wallRayDistance;
            playerMask      = type.playerMask;
            viewDistance    = type.viewDistance;

            currentHealth   = type.maxHealth;
        }
        else
        {
            if (moveDirection == 0) moveDirection = 1;
            Debug.LogWarning($"EnemyController en {name} no tiene EnemyType asignado.", this);
        }

        AutoFindPlayer();
    }


    public void Move(int dir, float speed)
    {
        if (dir == 0) return;
        tr.Translate(Vector3.right * dir * speed * Time.deltaTime, Space.World);
    }

    public bool DetectWallAhead()
    {
        Vector2 origin = tr.position;
        Vector2 dir = moveDirection >= 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, wallRayDistance, wallMask);
        return hit.collider != null;
    }

    public void NudgeBack(float amount = 0.01f)
    {
        float back = -Mathf.Sign(moveDirection) * Mathf.Abs(amount);
        tr.position += new Vector3(back, 0f, 0f);
    }

    public void FlipToDirection(int dir)
    {
        if (dir == 0) return;
        moveDirection = (int)Mathf.Sign(dir);

        Vector3 s = tr.localScale;
        if (Mathf.Sign(s.x) != moveDirection)
        {
            s.x *= -1f;
            tr.localScale = s;
        }
    }

    public bool CanSeePlayer()
    {
        if (!player) AutoFindPlayer();
        if (!player) return false;

        Vector2 origin = tr.position;
        Vector2 toPlayer = (Vector2)player.position - origin;
        float dist = toPlayer.magnitude;
        if (dist > viewDistance) return false;

        float facing = Mathf.Sign(moveDirection);
        if (Mathf.Sign(toPlayer.x) != facing) return false;

        RaycastHit2D hit = Physics2D.Raycast(origin, toPlayer.normalized, dist, playerMask);
        return hit.collider != null;
    }

    public int DirectionToPlayer()
    {
        if (!player) AutoFindPlayer();
        if (!player) return moveDirection;
        return player.position.x >= tr.position.x ? 1 : -1;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void AutoFindPlayer()
    {
        if (player) return;
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go) player = go.transform;
    }

}