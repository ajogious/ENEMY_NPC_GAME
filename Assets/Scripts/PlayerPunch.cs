//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerPunch : MonoBehaviour
//{
//    public float punchDamage = 10f;           // Damage dealt by the punch
//    public Collider punchCollider;            // Trigger collider (should be on a child object like the fist)

//    void Start()
//    {
//        if (punchCollider != null)
//        {
//            punchCollider.enabled = false;   // Initially disable trigger
//        }
//        else
//        {
//            Debug.LogError("PunchCollider is not assigned!");
//        }
//    }

//    void Update()
//    {
//        // Trigger punch on Spacebar
//        if (Keyboard.current.spaceKey.wasPressedThisFrame)
//        {
//            PerformPunch();
//        }
//    }

//    void PerformPunch()
//    {
//        if (punchCollider != null)
//        {
//            punchCollider.enabled = true;
//            Debug.Log("Player punches");

//            // Disable after a short time
//            Invoke(nameof(DisablePunchCollider), 0.2f);
//        }
//    }

//    void DisablePunchCollider()
//    {
//        if (punchCollider != null)
//        {
//            punchCollider.enabled = false;
//        }
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        Debug.Log("Punch Trigger Entered: " + other.name);  // Debug hit

//        if (other.CompareTag("Enemy"))
//        {
//            HealthSystem health = other.GetComponent<HealthSystem>();
//            if (health != null)
//            {
//                health.TakeDamage(punchDamage);
//                Debug.Log("Enemy hit for " + punchDamage + " damage.");
//            }
//            else
//            {
//                Debug.LogWarning("Enemy hit but no HealthSystem component found.");
//            }
//        }
//    }
//}
