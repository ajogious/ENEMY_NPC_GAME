using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Vision Settings")]
    public Transform visionOrigin;
    public Transform player;
    public float viewDistance = 10f;
    [Range(0, 360)] public float viewAngle = 60f;
    public LayerMask obstacleMask;

    public enum AwarenessState { Unaware, Suspicious, Alerted, Engaged }
    public AwarenessState currentState = AwarenessState.Unaware;

    private Renderer enemyRenderer;

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        UpdateAwareness();
    }

    // Main perception logic
    void UpdateAwareness()
    {
        if (CanSeePlayer())
        {
            SetState(AwarenessState.Engaged, Color.red, "Engaged");
        }
        else
        {
            SetState(AwarenessState.Unaware, Color.white, "Unaware");
        }
    }

    // Set state, change color and log message
    void SetState(AwarenessState newState, Color color, string logMessage)
    {
        if (currentState == newState) return;

        currentState = newState;
        if (enemyRenderer != null)
            enemyRenderer.material.color = color;

        Debug.Log($"Enemy State: {logMessage}");
    }

    // Check if player is visible to the enemy
    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - visionOrigin.position).normalized;
        float angle = Vector3.Angle(visionOrigin.forward, directionToPlayer);

        // Check angle
        if (angle > viewAngle / 2f) return false;

        float distance = Vector3.Distance(visionOrigin.position, player.position);

        // Check if line of sight is blocked
        if (Physics.Raycast(visionOrigin.position, directionToPlayer, distance, obstacleMask))
            return false;

        // Player is visible
        return distance <= viewDistance;
    }

    // Optional: Visualize vision cone in editor
    void OnDrawGizmosSelected()
    {
        if (visionOrigin == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(visionOrigin.position, viewDistance);

        Vector3 leftRay = Quaternion.Euler(0, -viewAngle / 2, 0) * visionOrigin.forward * viewDistance;
        Vector3 rightRay = Quaternion.Euler(0, viewAngle / 2, 0) * visionOrigin.forward * viewDistance;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(visionOrigin.position, leftRay);
        Gizmos.DrawRay(visionOrigin.position, rightRay);
    }
}
