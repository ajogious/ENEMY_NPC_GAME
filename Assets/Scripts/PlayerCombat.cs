using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float punchRange = 1.5f;
    public int punchDamage = 10;
    public LayerMask enemyLayer;

    public void TryPunch()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward, punchRange, enemyLayer);

        foreach (Collider hit in hits)
        {
            HealthSystem enemyHealth = hit.GetComponent<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(punchDamage, enemyHealth.isBlocking);
                Debug.Log("Enemy hit! Damage dealt.");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, punchRange);
    }
}
