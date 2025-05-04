using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRadius = 10f;
    [Range(0, 360)] public float viewAngle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public bool canSeePlayer;
    public Transform player;

    private void Update()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2f)
        {
            if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
            {
                if (distanceToPlayer <= viewRadius)
                {
                    canSeePlayer = true;
                    return;
                }
            }
        }

        canSeePlayer = false;
    }
}
