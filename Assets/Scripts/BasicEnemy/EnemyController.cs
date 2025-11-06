using UnityEngine;

[DisallowMultipleComponent]
public class EnemyController : MonoBehaviour
{
    [Header("Movimiento")]
    public float patrolSpeed = 3f;
    public float pursueSpeed = 5f;
    [Range(-1, 1)] public int moveDirection = 1; // 1 derecha, -1 izquierda
    [SerializeField] private bool flipWithScale = true;

    [Header("Detección de paredes")]
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float wallRayDistance = 1f;

    [Header("Visión del jugador")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float viewDistance = 8f;
    [SerializeField] private Transform player; // opcional, si está vacío busca por Tag "Player"

    // Cache
    private Transform tr;

    private void Awake()
    {
        tr = transform;
        if (moveDirection == 0) moveDirection = 1;
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
        var hit = Physics2D.Raycast(origin, dir, wallRayDistance, wallMask);
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
        // Mathf.Sign devuelve float (-1, 0, 1). Convertimos de forma segura a int.
        moveDirection = (int)Mathf.Sign(dir);
        if (!flipWithScale) return;

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
        Vector2 toPlayer = ((Vector2)player.position - origin);
        float dist = toPlayer.magnitude;
        if (dist > viewDistance) return false;

        // Solo “ve” hacia delante
        float facing = Mathf.Sign(moveDirection);
        if (Mathf.Sign(toPlayer.x) != facing) return false;

        // Raycast hacia el player en su capa
        var hit = Physics2D.Raycast(origin, toPlayer.normalized, dist, playerMask);
        return hit.collider != null;
    }

    public int DirectionToPlayer()
    {
        if (!player) AutoFindPlayer();
        if (!player) return moveDirection;
        return (player.position.x >= tr.position.x) ? 1 : -1;
    }

    private void AutoFindPlayer()
    {
        if (player) return;
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go) player = go.transform;
    }

    private void OnDrawGizmosSelected()
    {
        // Pared
        Gizmos.color = Color.red;
        Vector3 dir = (moveDirection >= 0 ? Vector3.right : Vector3.left) * wallRayDistance;
        Gizmos.DrawLine(transform.position, transform.position + dir);

        // Visión
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * viewDistance);
    }
}