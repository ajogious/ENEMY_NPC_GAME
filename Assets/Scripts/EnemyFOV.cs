using UnityEngine;

/// <summary>
/// Handles field of view detection for an enemy.
/// Determines whether the player is visible based on angle, distance, and obstacles.
/// </summary>
public class EnemyFOV : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewRadius = 10f;
    [Range(0, 360)] public float viewAngle = 90f;

    [Header("Detection Layers")]
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    [Header("References")]
    public Transform player;

    [HideInInspector] public bool canSeePlayer;

    private void Update()
    {
        CheckPlayerVisibility();
    }

    /// <summary>
    /// Checks if the player is within view angle, range, and not blocked by obstacles.
    /// </summary>
    private void CheckPlayerVisibility()
    {
        if (player == null)
        {
            canSeePlayer = false;
            return;
        }

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        bool isWithinViewAngle = Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2f;
        bool isInRange = distanceToPlayer <= viewRadius;
        bool hasLineOfSight = !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask);

        // All conditions must be met to see the player
        canSeePlayer = isWithinViewAngle && isInRange && hasLineOfSight;
    }

    /// <summary>
    /// Converts an angle in degrees to a direction vector.
    /// Useful for drawing vision cones.
    /// </summary>
    public Vector3 DirFromAngle(float angleInDegrees, bool isGlobal)
    {
        if (!isGlobal)
            angleInDegrees += transform.eulerAngles.y;

        float rad = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2, false);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

        if (canSeePlayer)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }

}
