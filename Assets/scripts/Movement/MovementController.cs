using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementForce = 50.0f;        // Force applied to the player for sensor-based movement
    public float forwardForce = 10.0f;         // Constant forward force applied to the player
    public float drag = 0.5f;                  // Drag applied to the rigidbody to smooth movement
    public int forceDifferenceThreshold = 100; // Threshold for directional movement
    public Camera mainCamera;                  // Reference to the main camera for boundary check

    private Rigidbody rb;                      // Rigidbody component
    private ArduinoInput sensorInput;      // Reference to the ForceSensorInput script

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
            }
            else
            {
                // Case 2: Differences are large, move based on sensor forces
                MoveBasedOnForces(forceUp, forceDown, forceLeft, forceRight);
            }
        }

        // Constrain the player's movement within the camera view
        ConstrainMovementWithinCamera();
    }

    // Method to apply constant forward movement
    void MoveForward()
    {
        Vector3 forwardForceVector = Vector3.forward * forwardForce;
        rb.AddForce(forwardForceVector, ForceMode.Acceleration);
    }

    // Method to apply movement based on the sensor forces
    void MoveBasedOnForces(int forceUp, int forceDown, int forceLeft, int forceRight)
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
