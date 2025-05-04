using UnityEngine;
using UnityEngine.InputSystem; // ✅ Required for new Input System

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackDamage = 20f;

    void Update()
    {
        // ✅ Using the new Input System directly
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Attack triggered");
            Attack();
        }
    }

    void Attack()
    {
        // ✅ Check for enemies within attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);
        Debug.Log("Enemies in range: " + hitEnemies.Length);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyFSM fsm = enemy.GetComponent<EnemyFSM>();
            if (fsm != null)
            {
                fsm.TakeDamage(attackDamage);
                Debug.Log("Hit enemy for " + attackDamage + " damage!");
            }
        }

        // ✅ Inform tracker that an attack happened
        var tracker = GetComponent<PlayerBehaviorTracker>();
        if (tracker != null)
        {
            tracker.IncrementAttack();
        }
    }

    void OnDrawGizmosSelected()
    {
        // ✅ Visualize attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
