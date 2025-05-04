using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles simple waypoint-based patrolling for the enemy.
/// Can be reused or extended as part of an FSM or modular AI system.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    [Tooltip("Points the enemy will patrol between in order.")]
    public Transform[] patrolPoints;

    private int currentPoint = 0;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points assigned for " + gameObject.name);
            enabled = false;
            return;
        }

        MoveToNextPatrolPoint();
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            MoveToNextPatrolPoint();
    }

    private void MoveToNextPatrolPoint()
    {
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
        agent.destination = patrolPoints[currentPoint].position;
    }

    /// <summary>
    /// Resets patrol to first point (optional external use).
    /// </summary>
    public void ResetPatrol()
    {
        currentPoint = 0;
        agent.destination = patrolPoints[currentPoint].position;
    }
}
