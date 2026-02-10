using UnityEngine;
using Color = UnityEngine.Color;

public class EnemyAI : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float patrolDistance = 3.0f;

    [Header("Detección (Cono de visión)")]
    [SerializeField] private float viewDistance = 4.0f;          
    [Range(0f, 360f)]
    [SerializeField] private float fov = 90.0f;                  
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;            

    [Header("Feedback")]
    [SerializeField] private SpriteRenderer alarmRenderer;       
    [SerializeField] private Color alertColor = Color.red;
    [SerializeField] private VisionConeRenderer cone;

    private Rigidbody2D rb;
    private Animator animator;

    private Color enemyColor;
    private Vector2 startPosition;
    private Vector2 targetPosition;

    private Transform playerTransform;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (!alarmRenderer) alarmRenderer = GetComponent<SpriteRenderer>();
        enemyColor = alarmRenderer.color;

        startPosition = transform.position;
        targetPosition = startPosition + Vector2.right * patrolDistance;
        if (!cone) cone = GetComponentInChildren<VisionConeRenderer>();
    }

    void FixedUpdate()
    {
        DetectPlayerCone();

        if (isChasing) ChasePlayer();
        else Patrol();
    }


    void Patrol()
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * speed;

        if (Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            targetPosition = (targetPosition == startPosition)
                ? startPosition + Vector2.right * patrolDistance
                : startPosition;
        }

        UpdateAnimation(direction);
    }

    void ChasePlayer()
    {
        if (!playerTransform)
        {
            StopChasing();
            return;
        }

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * (speed * 1.5f);

        UpdateAnimation(direction);
    }

    void StopChasing()
    {
        alarmRenderer.color = enemyColor;
        isChasing = false;
        playerTransform = null;
        if (cone) cone.SetAlert(false);
    }

    void DetectPlayerCone()
    {
        Transform candidate = PlayerInRange();
        if (!candidate) { StopChasing(); return; }

        if (!PlayerInAngle(candidate)) { StopChasing(); return; }

        if (!PlayerVisible(candidate)) { StopChasing(); return; }

        playerTransform = candidate;
        alarmRenderer.color = alertColor;
        isChasing = true;
        if (cone) cone.SetAlert(true);
    }

    Transform PlayerInRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, viewDistance, playerLayer);
        return hit ? hit.transform : null;
    }

    bool PlayerInAngle(Transform player)
    {
        Vector2 toPlayer = (player.position - transform.position);
        Vector2 forward = (transform.localScale.x >= 0f) ? Vector2.right : Vector2.left;

        float angle = Vector2.Angle(forward, toPlayer);
        return angle <= (fov * 0.5f);
    }

    bool PlayerVisible(Transform player)
    {
        Vector2 origin = transform.position;
        Vector2 toPlayer = (player.position - transform.position);
        float dist = toPlayer.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(origin, toPlayer.normalized, dist, obstacleLayer | playerLayer);

        if (!hit.collider) return false;

        int hitLayerBit = 1 << hit.collider.gameObject.layer;
        return (hitLayerBit & playerLayer) != 0;
    }

    void UpdateAnimation(Vector2 dir)
    {
        if (animator == null) return;

        animator.SetFloat("horizontal", dir.x);
        animator.SetFloat("vertical", dir.y);
        animator.SetBool("isMoving", true);

        if (Mathf.Abs(dir.x) > 0.1f)
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector2 forward = (transform.localScale.x >= 0f) ? Vector2.right : Vector2.left;
        float half = fov * 0.5f;

        Vector2 leftDir = Rotate2D(forward, -half);
        Vector2 rightDir = Rotate2D(forward, half);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + leftDir * viewDistance);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + rightDir * viewDistance);
    }

    private Vector2 Rotate2D(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}
