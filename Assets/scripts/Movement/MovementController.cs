using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5.0f;     // Speed at which the character moves forward
    [SerializeField] private float rotationSpeed = 2.0f;    // Speed at which the character rotates
    [SerializeField] private float movementSpeed = 5.0f;    // Speed at which the character moves left/right/up/down
    [SerializeField] private float smoothingFactor = 0.1f;  // Smoothing factor for directional changes

    private Vector2 moveInput;            // Input from the New Input System for left/right and up/down movement
    private Vector3 targetDirection;      // The calculated direction based on input
    private Vector3 smoothedDirection;    // The direction that is smoothed over time

    public InputActionReference move;

    private void Start()
    {
        // Initialize the direction variables
        targetDirection = transform.forward;
        smoothedDirection = transform.forward;
    }

    void FixedUpdate()
    {
        // Calculate the target direction based on player input
        targetDirection = new Vector3(moveInput.x, moveInput.y, 1.0f);
        targetDirection = targetDirection.normalized;

        // Smooth the transition to the target direction using Slerp
        smoothedDirection = Vector3.Slerp(smoothedDirection, targetDirection, smoothingFactor);

        // Rotate towards the smoothed direction
        Quaternion targetRotation = Quaternion.LookRotation(smoothedDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move the character forward constantly
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Apply additional movement for left/right and up/down directions
        transform.Translate(new Vector3(moveInput.x, moveInput.y, 0) * movementSpeed * Time.deltaTime, Space.Self);
    }

    // Input System Callback for receiving Move input
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
