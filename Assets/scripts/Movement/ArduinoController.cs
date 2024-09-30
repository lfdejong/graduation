using UnityEngine;

public class ArduinoController : MonoBehaviour
{
    public float movementSpeed = 5.0f;      // Movement speed
    public float rotationSpeed = 2.0f;      // Speed at which the character rotates
    public float maxHorizontalRange = 1.0f; // Max horizontal movement range (-1 to 1)
    public float maxVerticalRange = 1.0f;   // Max vertical movement range (-1 to 1)
    public float pressureThresholdLow = 100.0f;  // Low pressure threshold for light movement
    public float pressureThresholdHigh = 2000.0f; // High pressure threshold for stronger movement
    public float smoothingFactor = 0.1f;    // How smooth the transition between directions is
    public Camera mainCamera;                // Reference to the main camera for boundary check
    public ArduinoInput arduinoInput;

    private Vector3 targetDirection = Vector3.zero;  // Target movement direction
    private Vector3 smoothedDirection = Vector3.zero; // Smoothed direction for movement

    //arduino input
    private float force1;
    private float force2;
    private float force3;
    private float force4;

    private enum MovementState
    {
        NoMovement,
        LightMovement,
        StrongMovement
    }

    void Start()
    {
        // Initialize movement directions
        targetDirection = transform.forward;
        smoothedDirection = transform.forward;


    }

    void Update()
    {

                    // Calculate the total pressure from all sensors
                    float totalPressure = force1 + force2 + force3 + force4;

                    UnityEngine.Debug.Log(totalPressure);
                    UnityEngine.Debug.Log("force 1: " + force1);
                    UnityEngine.Debug.Log("force 2: " + force2);
                    UnityEngine.Debug.Log("force 3: " + force3);
                    UnityEngine.Debug.Log("force 4: " + force4);


                    // Determine movement state based on total pressure using a switch case
                    MovementState movementState = DetermineMovementState(totalPressure);

                    switch (movementState)
                    {
                        case MovementState.NoMovement:
                            //constant movement
                            UnityEngine.Debug.Log("No movement");
                            transform.position += transform.forward * (movementSpeed * Time.smoothDeltaTime);
                            return;
                        case MovementState.LightMovement:
                            //constant movement
                            UnityEngine.Debug.Log("Light movement");
                            float horizontalInput = (force4 - force3) / 1; // Normalized to range -1 to 1
                            float verticalInput = (force2 - force1) / 1;   // Normalized to range -1 to 1

                            horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);
                            verticalInput = Mathf.Clamp(verticalInput, -1, 1);

                            UnityEngine.Debug.Log(horizontalInput + ", " + verticalInput);

                            targetDirection = new Vector3(horizontalInput * maxHorizontalRange, verticalInput * maxVerticalRange, 1.0f).normalized;

                            // Smooth the transition to the target direction using Slerp
                            smoothedDirection = Vector3.Slerp(smoothedDirection, targetDirection, smoothingFactor);


                            // Move the player in the direction it is facing (forward) and apply additional movement based on input
                            transform.Translate(smoothedDirection * movementSpeed * Time.smoothDeltaTime, Space.World);
                            transform.position += transform.forward * (movementSpeed * Time.smoothDeltaTime);
                            break;
                        case MovementState.StrongMovement:
                            UnityEngine.Debug.Log("Strong movement");
                            // Normalize input to calculate horizontal and vertical directions
                            horizontalInput = (force4 - force3) / 2; // Normalized to range -1 to 1
                            verticalInput = (force2 - force1) / 2;   // Normalized to range -1 to 1

                            horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);
                            verticalInput = Mathf.Clamp(verticalInput, -1, 1);

                            UnityEngine.Debug.Log(horizontalInput + ", " + verticalInput);

                            targetDirection = new Vector3(horizontalInput * maxHorizontalRange, verticalInput * maxVerticalRange, 1.0f).normalized;

                            // Smooth the transition to the target direction using Slerp
                            smoothedDirection = Vector3.Slerp(smoothedDirection, targetDirection, smoothingFactor);


                            // Move the player in the direction it is facing (forward) and apply additional movement based on input
                            transform.Translate(smoothedDirection * movementSpeed * Time.smoothDeltaTime, Space.World);
                            transform.position += transform.forward * (movementSpeed * Time.smoothDeltaTime);
                            break;
                    }
                
            
        // Constrain the player's movement within the camera view
       ConstrainMovementWithinCamera();
    }

    private MovementState DetermineMovementState(float totalPressure)
    {
        // Use a switch statement to determine the movement state based on pressure
        switch (totalPressure)
        {
            case float p when p < pressureThresholdLow:
                return MovementState.NoMovement;

            case float p when p >= pressureThresholdLow && p < pressureThresholdHigh:
                return MovementState.LightMovement;

            case float p when p >= pressureThresholdHigh:
                return MovementState.StrongMovement;

            default:
                return MovementState.NoMovement;
        }
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
