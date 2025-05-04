using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState { Patrolling, Chasing, Attacking, Retreating, Searching }
    public EnemyState currentState = EnemyState.Patrolling;

    public Transform[] patrolPoints;
    private int patrolIndex = 0;
    private NavMeshAgent agent;
    public Transform player;

    // Ranges and thresholds
    public float attackRange = 2f;
    public float chaseRange = 10f;
    public float retreatThreshold = 15f;
    public float lowHealth = 30f;

    private float health = 100f;
    private float lastHitTime = 0f;
    public float hitCooldown = 2f;

    // Search behavior
    private float searchTimer = 0f;
    public float searchDuration = 5f;
    private Vector3 lastKnownPlayerPosition;

    // Adaptation system
    private float aggressiveScore = 0f;
    private float stealthScore = 0f;
    private float scoreDecayRate = 1f;

    // Dependencies
    public EnemyFOV fov;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Adaptation updates
        UpdateBehaviorScores();

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                if (fov.canSeePlayer && distanceToPlayer < chaseRange)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Chasing:
                Chase();
                if (distanceToPlayer < attackRange)
                    currentState = EnemyState.Attacking;
                else if (!fov.canSeePlayer)
                {
                    lastKnownPlayerPosition = player.position;
                    currentState = EnemyState.Searching;
                }
                break;

            case EnemyState.Attacking:
                Attack();
                if (distanceToPlayer > attackRange)
                    currentState = EnemyState.Chasing;
                if (health < lowHealth)
                    currentState = EnemyState.Retreating;
                break;

            case EnemyState.Retreating:
                Retreat();
                if (Vector3.Distance(transform.position, player.position) > retreatThreshold)
                {
                    health += 10f;
                    if (health > 100f) health = 100f;
                    currentState = EnemyState.Patrolling;
                }
                break;

            case EnemyState.Searching:
                Search();
                if (fov.canSeePlayer)
                    currentState = EnemyState.Chasing;
                break;
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[patrolIndex].position;
    }

    void Chase()
    {
        agent.destination = player.position;
    }

    void Attack()
    {
        agent.ResetPath();
        transform.LookAt(player);
        Debug.Log("Enemy is attacking!");
    }

    void Retreat()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 retreatPos = transform.position + dir * 5f;
        agent.destination = retreatPos;
        Debug.Log("Enemy is retreating!");
    }

    void Search()
    {
        agent.destination = lastKnownPlayerPosition;
        searchTimer += Time.deltaTime;
        if (searchTimer >= searchDuration)
        {
            searchTimer = 0f;
            currentState = EnemyState.Patrolling;
        }
        Debug.Log("Enemy is searching...");
    }

    public void TakeDamage(float amount)
    {
        if (Time.time - lastHitTime < hitCooldown)
            return;

        lastHitTime = Time.time;
        health -= amount;
        if (health < 0) health = 0;

        Debug.Log("Enemy took damage: " + amount + ", Current health: " + health);

        // Increase aggressiveScore when player attacks
        aggressiveScore += 10f;

        if (health < lowHealth && currentState != EnemyState.Retreating)
        {
            currentState = EnemyState.Retreating;
            Debug.Log("Enemy health low! Retreating.");
        }
    }

    void UpdateBehaviorScores()
    {
        if (fov.canSeePlayer)
        {
            // Player is visible but not attacking
            stealthScore += Time.deltaTime * 2f;
        }

        // Decay both scores over time
        aggressiveScore = Mathf.Max(0, aggressiveScore - Time.deltaTime * scoreDecayRate);
        stealthScore = Mathf.Max(0, stealthScore - Time.deltaTime * scoreDecayRate);

        // Adapt tactics based on scores
        if (stealthScore > 15f)
        {
            chaseRange = 15f; // Be more aggressive in spotting
            Debug.Log("Enemy is adapting to stealthy player. Increased chase range.");
        }

        if (aggressiveScore > 20f)
        {
            retreatThreshold = 10f; // Retreat sooner
            Debug.Log("Enemy is adapting to aggressive player. Retreats earlier.");
        }
    }
}
