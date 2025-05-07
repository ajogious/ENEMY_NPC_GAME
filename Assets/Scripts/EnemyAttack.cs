//using UnityEngine;

//public class EnemyAttack : MonoBehaviour
//{
//    [Header("Attack Settings")]
//    public float attackRange = 1.5f;
//    public float attackDamage = 15f;
//    public float attackCooldown = 1.5f;

//    [Header("References")]
//    public LayerMask playerLayer;
//    public Transform attackPoint;

//    private float lastAttackTime = -Mathf.Infinity;

//    void Update()
//    {
//        // Only try to attack if cooldown has passed
//        if (Time.time - lastAttackTime < attackCooldown)
//            return;

//        // Detect any player colliders in range
//        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
//        foreach (Collider hit in hits)
//        {
//            // Attack!
//            lastAttackTime = Time.time;
//            Debug.Log("Enemy Punch!");

//            HealthSystem playerHealth = hit.GetComponent<HealthSystem>();
//            if (playerHealth != null)
//            {
//                playerHealth.TakeDamage(attackDamage);
//            }
//            else
//            {
//                Debug.LogWarning("Hit object has no HealthSystem: " + hit.name);
//            }

//            // If you only want one hit per cooldown, break here:
//            break;
//        }
//    }

//    // Draws the attack range in the editor
//    private void OnDrawGizmosSelected()
//    {
//        if (attackPoint == null) return;

//        Gizmos.color = Color.blue;
//        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
//    }
//}
