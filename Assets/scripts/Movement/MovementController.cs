using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] public float movementForce;        // Force applied to the player for sensor-based movement
    [SerializeField] public float forwardForce;         // Constant forward force applied to the player
    [SerializeField] private float drag = 0.5f;                  // Drag applied to the rigidbody to smooth movement
    [SerializeField] private int forceDifferenceThreshold = 100; // Threshold for directional movement

    [SerializeField] private float rotationSpeed = 2.0f;  // Speed at which the plane rotates
    [SerializeField] private float maxPitchAngle = 30f;   // Max rotation angle for pitch (up/down)
    [SerializeField] private float maxRollAngle = 30f;    // Max rotation angle for roll (left/right)

    public Camera mainCamera;                  // Reference to the main camera for boundary check
    public PressureUI PUI; // Reference to the PressureUI script

    private Rigidbody rb;                      // Rigidbody component
    private ArduinoInput sensorInput;          // Reference to the ForceSensorInput script

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Set Rigidbody's interpolation mode to smooth movement between frames
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.drag = drag;  // Set drag to smooth out movement

        // Get the ForceSensorInput component from the same GameObject
        sensorInput = GetComponent<ArduinoInput>();
    }

    void FixedUpdate()
    {
        // Ensure the force sensor input is available before moving
        if (sensorInput != null)
        {
            // Get the forces from each sensor
            int forceUp = sensorInput.GetForceUp();
            int forceDown = sensorInput.GetForceDown();
            int forceLeft = sensorInput.GetForceLeft();
            int forceRight = sensorInput.GetForceRight();

            // Calculate the differences between opposite sensors
            int verticalDifference = Mathf.Abs(forceUp - forceDown);
            int horizontalDifference = Mathf.Abs(forceLeft - forceRight);

            // Determine movement based on the force differences
            if (verticalDifference < forceDifferenceThreshold && horizontalDifference < forceDifferenceThreshold)
            {
                // Case 1: Differences are small, move forward
                MoveForward();
                PUI.UpdateUIFeedback(-1); // Neutral feedback
            }
            else
            {
                // Case 2: Differences are large, move based on sensor forces
                int activeSensor = GetActiveSensor(forceUp, forceDown, forceLeft, forceRight);
                MoveBasedOnForces(forceUp, forceDown, forceLeft, forceRight);
                PUI.UpdateUIFeedback(activeSensor); // Update UI feedback based on active sensor
            }
        }

        // Rotate the plane based on the current movement direction
        RotateTowardsMovement();
        ConstrainMovementWithinCamera();
    }

    // Method to apply constant forward movement
    private void MoveForward()
    {
        Vector3 forwardForceVector = Vector3.forward * forwardForce;
        rb.AddForce(forwardForceVector, ForceMode.Acceleration);
    }

    // Method to apply movement based on the sensor forces
    private void MoveBasedOnForces(int forceUp, int forceDown, int forceLeft, int forceRight)
    {
        Vector3 targetForce = Vector3.zero;

        // Apply vertical forces
        if (forceUp > forceDown)
        {
            targetForce.y = (forceUp - forceDown) / 2046.0f;
        }
        else if (forceDown > forceUp)
        {
            targetForce.y = -(forceDown - forceUp) / 2046.0f;
        }

        // Apply horizontal forces
        if (forceRight > forceLeft)
        {
            targetForce.x = (forceRight - forceLeft) / 2046.0f;
        }
        else if (forceLeft > forceRight)
        {
            targetForce.x = -(forceLeft - forceRight) / 2046.0f;
        }

        // Normalize the direction and apply the movement force
        rb.AddForce(targetForce.normalized * movementForce, ForceMode.Acceleration);
    }

    // Method to determine the active sensor based on force differences
    int GetActiveSensor(int forceUp, int forceDown, int forceLeft, int forceRight)
    {
        int[] forces = { forceUp, forceDown, forceLeft, forceRight };
        int maxForce = Mathf.Max(forces);

        if (maxForce < forceDifferenceThreshold)
            return -1; // Neutral state

        // Find the index of the highest force (0: Up, 1: Down, 2: Left, 3: Right)
        for (int i = 0; i < forces.Length; i++)
        {
            if (forces[i] == maxForce)
                return i;
        }
        return -1; // Fallback to neutral if no force stands out
    }

    // Method to rotate the plane based on the current movement direction
    private void RotateTowardsMovement()
    {
        // Calculate desired rotation based on the current velocity
        Vector3 velocity = rb.velocity;

        // Calculate pitch angle (X-axis) based on vertical movement (up/down)
        float pitch = Mathf.Clamp(-velocity.y * rotationSpeed, -maxPitchAngle, maxPitchAngle);

        // Calculate roll angle (Z-axis) based on horizontal movement (left/right)
        float roll = Mathf.Clamp(velocity.x * rotationSpeed, -maxRollAngle, maxRollAngle);

        // Construct the target rotation using the calculated pitch and roll angles
        Quaternion targetRotation = Quaternion.Euler(pitch, 0, roll);

        // Smoothly interpolate towards the target rotation
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    // Ensure the player's movement stays within the camera bounds
    void ConstrainMovementWithinCamera()
    {
        // Get the player's position in screen space
        Vector3 screenPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the player is outside the screen bounds and clamp the position
        screenPosition.x = Mathf.Clamp01(screenPosition.x);
        screenPosition.y = Mathf.Clamp01(screenPosition.y);

        // Convert the clamped screen position back to world space and set the player's position
        transform.position = mainCamera.ViewportToWorldPoint(screenPosition);
    }
}
