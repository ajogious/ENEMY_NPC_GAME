using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls enemy behavior using a Finite State Machine (FSM).
/// Handles patrolling, chasing, attacking, retreating, and searching.
/// Adapts to player behavior over time.
/// </summary>
public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState { Patrolling, Chasing, Attacking, Retreating, Searching }
    public EnemyState currentState = EnemyState.Patrolling;

    [Header("Navigation")]
    public Transform[] patrolPoints;
    private int patrolIndex = 0;
    private NavMeshAgent agent;

    [Header("Player")]
    public Transform player;

    [Header("Combat Parameters")]
    public float attackRange = 2f;
    public float chaseRange = 10f;
    public float retreatThreshold = 15f;
    public float lowHealth = 30f;

    [Header("Health")]
    private float health = 100f;
    private float lastHitTime = 0f;
    public float hitCooldown = 2f;

    [Header("Search Settings")]
    private float searchTimer = 0f;
    public float searchDuration = 5f;
    private Vector3 lastKnownPlayerPosition;

    [Header("Adaptive Behavior")]
    private float aggressiveScore = 0f;
    private float stealthScore = 0f;
    public float scoreDecayRate = 1f;

    [Header("Dependencies")]
    public EnemyFOV fov;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextPatrolPoint();
    }

    private void Update()
    {
        if (player == null || agent == null || fov == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        UpdateBehaviorScores();

        switch (currentState)
        {
            case EnemyState.Patrolling:
                HandlePatrolling(distanceToPlayer);
                break;

            case EnemyState.Chasing:
                HandleChasing(distanceToPlayer);
                break;

            case EnemyState.Attacking:
                HandleAttacking(distanceToPlayer);
                break;

            case EnemyState.Retreating:
                HandleRetreating(distanceToPlayer);
                break;

            case EnemyState.Searching:
                HandleSearching();
                break;
        }
    }

    private void HandlePatrolling(float distance)
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextPatrolPoint();

        if (fov.canSeePlayer && distance < chaseRange)
            TransitionToState(EnemyState.Chasing);
    }

    private void HandleChasing(float distance)
    {
        agent.destination = player.position;

        if (distance < attackRange)
            TransitionToState(EnemyState.Attacking);
        else if (!fov.canSeePlayer)
        {
            lastKnownPlayerPosition = player.position;
            TransitionToState(EnemyState.Searching);
        }
    }

    private void HandleAttacking(float distance)
    {
        agent.ResetPath();
        transform.LookAt(player);
        Debug.Log("Enemy is attacking!");

        if (distance > attackRange)
            TransitionToState(EnemyState.Chasing);
        else if (health < lowHealth)
            TransitionToState(EnemyState.Retreating);
    }

    private void HandleRetreating(float distance)
    {
        Vector3 dir = (transform.position - player.position).normalized;
        agent.destination = transform.position + dir * 5f;
        Debug.Log("Enemy is retreating!");

        if (distance > retreatThreshold)
        {
            health = Mathf.Min(health + 10f, 100f); // Passive regen on retreat
            TransitionToState(EnemyState.Patrolling);
        }
    }

    private void HandleSearching()
    {
        agent.destination = lastKnownPlayerPosition;
        searchTimer += Time.deltaTime;

        Debug.Log("Enemy is searching...");

        if (searchTimer >= searchDuration)
        {
            searchTimer = 0f;
            TransitionToState(EnemyState.Patrolling);
        }

        if (fov.canSeePlayer)
            TransitionToState(EnemyState.Chasing);
    }

    private void TransitionToState(EnemyState newState)
    {
        currentState = newState;
        Debug.Log("Transitioning to: " + newState);
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        agent.destination = patrolPoints[patrolIndex].position;
    }

    public void TakeDamage(float amount)
    {
        if (Time.time - lastHitTime < hitCooldown)
            return;

        lastHitTime = Time.time;
        health = Mathf.Max(health - amount, 0f);

        aggressiveScore += 10f;
        Debug.Log($"Enemy took {amount} damage. Health: {health}");

        if (health < lowHealth && currentState != EnemyState.Retreating)
            TransitionToState(EnemyState.Retreating);
    }

    private void UpdateBehaviorScores()
    {
        if (fov.canSeePlayer)
            stealthScore += Time.deltaTime * 2f;

        // Decay over time
        aggressiveScore = Mathf.Max(0f, aggressiveScore - Time.deltaTime * scoreDecayRate);
        stealthScore = Mathf.Max(0f, stealthScore - Time.deltaTime * scoreDecayRate);

        // Adaptive tweaks
        if (stealthScore > 15f)
        {
            chaseRange = 15f;
            Debug.Log("Adapting to stealthy player: increased chase range.");
        }

        if (aggressiveScore > 20f)
        {
            retreatThreshold = 10f;
            Debug.Log("Adapting to aggressive player: lowered retreat threshold.");
        }
    }
}
