using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Vector2 moveInput;
    private CharacterController controller;
    private PlayerControls controls;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new PlayerControls();
        SetupControls();
    }

    private void SetupControls()
    {
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;
    }

    private void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move); // Optional: movement relative to player orientation
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
}
