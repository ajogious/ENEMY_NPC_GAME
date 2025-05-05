using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls enemy behavior using a Finite State Machine (FSM).
/// Includes adaptation using Genetic Algorithm via EnemyDNA.
/// </summary>
public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState { Patrolling, Chasing, Attacking, Retreating, Searching, Healing }
    public EnemyState currentState = EnemyState.Patrolling;

    [Header("Navigation")]
    public Transform[] patrolPoints;
    private int patrolIndex = 0;
    private NavMeshAgent agent;

    [Header("Player")]
    public Transform player;

    [Header("Combat Parameters")]
    public float defaultAttackRange = 2f;
    public float defaultChaseRange = 10f;
    public float defaultRetreatThreshold = 15f;
    public float lowHealth = 30f;

    [Header("Health")]
    private float health = 100f;
    private float lastHitTime = 0f;
    public float hitCooldown = 2f;

    [Header("Search Settings")]
    private float searchTimer = 0f;
    public float searchDuration = 5f;
    private Vector3 lastKnownPlayerPosition;

    [Header("Adaptive Behavior (optional scoring)")]
    private float aggressiveScore = 0f;
    private float stealthScore = 0f;
    public float scoreDecayRate = 1f;

    [Header("Dependencies")]
    public EnemyFOV fov;

    private EnemyGAController gaController;
    private EnemyDNA dna;
    private float lastAttackTime = 0f;

    private bool isRetreatingComplete = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextPatrolPoint();

        gaController = GetComponent<EnemyGAController>();
        dna = gaController?.GetCurrentDNA();
    }

    private void Update()
    {
        if (player == null || agent == null || fov == null)
            return;

        dna = gaController?.GetCurrentDNA();

        float chaseRange = dna != null ? dna.chaseRange : defaultChaseRange;
        float retreatThreshold = dna != null ? dna.chaseRange : defaultRetreatThreshold;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        UpdateBehaviorScores();

        if ((currentState == EnemyState.Patrolling || currentState == EnemyState.Searching) && fov.canSeePlayer)
        {
            TransitionToState(EnemyState.Chasing);
        }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                HandlePatrolling(distanceToPlayer, chaseRange);
                break;
            case EnemyState.Chasing:
                HandleChasing(distanceToPlayer);
                break;
            case EnemyState.Attacking:
                HandleAttacking(distanceToPlayer);
                break;
            case EnemyState.Retreating:
                HandleRetreating(distanceToPlayer, retreatThreshold);
                break;
            case EnemyState.Searching:
                HandleSearching();
                break;
            case EnemyState.Healing:
                HandleHealing();
                break;
        }
    }

    private void HandlePatrolling(float distance, float chaseRange)
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextPatrolPoint();

        if (fov.canSeePlayer && distance < chaseRange)
            TransitionToState(EnemyState.Chasing);
    }

    private void HandleChasing(float distance)
    {
        agent.destination = player.position;

        if (distance < defaultAttackRange)
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

        float aggression = dna != null ? dna.aggression : 0.5f;
        float attackCooldown = Mathf.Lerp(2f, 0.5f, aggression);

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log($"Enemy attacks with aggression: {aggression} (cooldown: {attackCooldown}s)");
            lastAttackTime = Time.time;
            // Add attack logic here (e.g., damage)
        }

        if (distance > defaultAttackRange)
            TransitionToState(EnemyState.Chasing);
        else if (health < lowHealth && currentState != EnemyState.Retreating)
            TransitionToState(EnemyState.Retreating);
    }

    private void HandleRetreating(float distance, float retreatThreshold)
    {
        transform.LookAt(player);

        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 retreatPoint = transform.position + dir * 10f;

        if (!agent.hasPath)
        {
            if (NavMesh.SamplePosition(retreatPoint, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                agent.destination = hit.position;
            }
        }

        Debug.Log("Enemy is retreating!");

        if (!isRetreatingComplete && distance > retreatThreshold)
        {
            TransitionToState(EnemyState.Healing);
        }
    }

    private void HandleHealing()
    {
        agent.ResetPath();  // Prevent movement
        Debug.Log("Enemy Healing...");

        // Heal over time
        health = Mathf.Min(health + Time.deltaTime * 10f, 100f);

        if (health >= 100f)
        {
            Debug.Log("Healing complete. Enemy will resume behavior.");

            if (gaController != null)
            {
                gaController.AdaptToPlayer();
                dna = gaController.GetCurrentDNA();
                Debug.Log("Enemy evolved new DNA after healing.");
            }

            isRetreatingComplete = true;
            searchTimer = 0f;
            TransitionToState(EnemyState.Searching);
        }
    }

    private void HandleSearching()
    {
        transform.LookAt(player);
        agent.destination = lastKnownPlayerPosition;
        searchTimer += Time.deltaTime;

        Debug.Log("Enemy is searching...");

        if (searchTimer >= searchDuration)
        {
            searchTimer = 0f;
            TransitionToState(EnemyState.Patrolling);
        }

        if (fov.canSeePlayer)
        {
            searchTimer = 0f;
            TransitionToState(EnemyState.Chasing);
        }
    }

    private void TransitionToState(EnemyState newState)
    {
        Debug.Log("Transitioning to: " + newState);

        currentState = newState;
        agent.ResetPath();

        if (newState == EnemyState.Retreating)
        {
            isRetreatingComplete = false;
        }
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
        {
            TransitionToState(EnemyState.Retreating);
        }
    }

    private void UpdateBehaviorScores()
    {
        if (fov.canSeePlayer)
            stealthScore += Time.deltaTime * 2f;

        aggressiveScore = Mathf.Max(0f, aggressiveScore - Time.deltaTime * scoreDecayRate);
        stealthScore = Mathf.Max(0f, stealthScore - Time.deltaTime * scoreDecayRate);

        if (stealthScore > 15f)
        {
            Debug.Log("Player is stealthy. Enemy considering wider chase range.");
        }

        if (aggressiveScore > 20f)
        {   
            Debug.Log("Player is aggressive. Enemy may evolve faster.");
        }
    }

}
