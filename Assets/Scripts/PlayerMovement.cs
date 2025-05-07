using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float rotateSpeed = 720f;

    [Header("Attack Settings")]
    public float punchDamage = 10f;
    public Transform punchPoint;
    public float punchRange = 1f;
    public LayerMask enemyLayer;

    private Rigidbody rb;
    private Vector2 moveInput;
    private PlayerController playerControls;

    private bool isBlocking = false;
    public bool isSprinting { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerControls = new PlayerController();

        // Register action bindings
        playerControls.Player.Punch.performed += ctx => Punch();
        playerControls.Player.Block.performed += ctx => StartBlocking();
        playerControls.Player.Block.canceled += ctx => StopBlocking();
    }

    void OnEnable() => playerControls.Player.Enable();
    void OnDisable() => playerControls.Player.Disable();

    void FixedUpdate()
    {
        if (isBlocking) return; // Prevent movement while blocking (optional mechanic)

        moveInput = playerControls.Player.Move.ReadValue<Vector2>();

        MovePlayer(moveInput);
    }

    private void MovePlayer(Vector2 input)
    {
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;
        isSprinting = Keyboard.current.leftShiftKey.isPressed;
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        if (moveDir == Vector3.zero) return;

        // Move and rotate
        rb.MovePosition(transform.position + moveDir * speed * Time.fixedDeltaTime);
        Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
    }

    private void Punch()
    {
        if (isBlocking) return;

        Debug.Log("Player punches!");

        Collider[] hitEnemies = Physics.OverlapSphere(punchPoint.position, punchRange, enemyLayer);
        Debug.Log($"Enemies hit: {hitEnemies.Length}");

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                HealthSystem enemyHealth = enemy.GetComponent<HealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(punchDamage, enemyHealth.isBlocking);
                }
            }
        }
    }

    private void StartBlocking()
    {
        isBlocking = true;
        Debug.Log("Player is blocking!");
        // TODO: Add animation or block effect here
    }

    private void StopBlocking()
    {
        isBlocking = false;
        Debug.Log("Player stopped blocking!");
        // TODO: End block animation or effect
    }

    void OnDrawGizmosSelected()
    {
        if (punchPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(punchPoint.position, punchRange);
        }
    }
}
