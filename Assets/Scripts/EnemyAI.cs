using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float patrolDistance = 3.0f; // DIstancia que se mueve desde el spawn

    [Header("Detección")]
    [SerializeField] private float detectionRange = 4.0f;
    [SerializeField] private LayerMask playerLayer; // Asigna la capa "Player" aquí

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private Transform playerTransform;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        // El primer punto de patrulla será a la derecha
        targetPosition = startPosition + Vector2.right * patrolDistance;
    }

    void FixedUpdate()
    {
        DetectPlayer();

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        // Moverse hacia el punto objetivo
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * speed;

        // Si llega al objetivo cambia
        if (Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            targetPosition = (targetPosition == startPosition)
                ? startPosition + Vector2.right * patrolDistance
                : startPosition;
        }

        UpdateAnimation(direction);
    }

    void DetectPlayer()
    {
        // El coso circular de detectar
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        if (hit != null)
        {
            playerTransform = hit.transform;
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized; // Posible casting Vector2
        rb.linearVelocity = direction * (speed * 1.5f); // Más rápido al perseguir

        UpdateAnimation(direction);
    }

    void UpdateAnimation(Vector2 dir)
    {
        if (animator == null) return;

        animator.SetFloat("horizontal", dir.x);
        animator.SetFloat("vertical", dir.y);
        animator.SetBool("isMoving", true);

        // Flip automático según dirección X
        if (Mathf.Abs(dir.x) > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
        }
    }

    // Para ver el rango de detección en el editor (solo visual)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.blue;
        if (Application.isPlaying) Gizmos.DrawLine(startPosition, startPosition + Vector2.right * patrolDistance);
    }
}
