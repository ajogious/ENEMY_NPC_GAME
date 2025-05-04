using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

/// <summary>
/// Handles player melee attack logic, using a radius-based hit detection.
/// </summary>
[RequireComponent(typeof(PlayerBehaviorTracker))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackDamage = 20f;

    private PlayerBehaviorTracker tracker;

    private void Awake()
    {
        // Cache component reference
        tracker = GetComponent<PlayerBehaviorTracker>();
    }

    private void Update()
    {
        // Trigger attack using the new Input System
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Attack triggered");
            Attack();
        }
    }

    /// <summary>
    /// Performs a melee attack, damaging nearby enemies.
    /// </summary>
    private void Attack()
    {
        // Get all colliders within the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);
        Debug.Log($"Enemies in range: {hitEnemies.Length}");

        int enemiesHit = 0;

        foreach (Collider enemy in hitEnemies)
        {
            EnemyFSM fsm = enemy.GetComponent<EnemyFSM>();
            if (fsm != null)
            {
                fsm.TakeDamage(attackDamage);
                enemiesHit++;
                Debug.Log($"Hit {enemy.name} for {attackDamage} damage.");
            }
        }

        // Log the attack event for tracking/player analytics
        if (enemiesHit > 0 && tracker != null)
        {
            tracker.IncrementAttack();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw attack range in editor when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
