using System.IO.Ports;
using UnityEngine;


public class ArduinoController : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM3", 9600); // Set the correct COM port
    [SerializeField] private float movementSpeed = 5.0f; // Speed at which the object moves
    [SerializeField] private float rotationSpeed = 2.0f; // Speed at which the object rotates
    [SerializeField] private float maxHorizontalRange = 2000f;// Maximum movement range on the X-axis
    [SerializeField] private float maxVerticalRange = 500.0f;//max movemeent range on the Y-axis
    [SerializeField] private float smoothingFactor = 0.1f; // Smoothing factor for input values

    //forward speed 

    public float forwardSpeed = 10.0f; 
    public float turnSpeed = 2.0f;      // Speed at which the object turns towards the target direction

    private Vector3 targetDirection;    // The direction the object should move towards
    private Vector3 smoothedDirection = Vector3.zero; // Smoothed direction for movement

    void Start()
    {
        // Initialize the serial port
        serialPort.Open();
        serialPort.ReadTimeout = 50;

        targetDirection = transform.forward;
    }

    void Update()
    {
        // Read force sensor values from Arduino
        if (serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();
                string[] sensorValues = data.Split(',');

                if (sensorValues.Length == 4)
                {
                    int force1 = int.Parse(sensorValues[0]);
                    int force2 = int.Parse(sensorValues[1]);
                    int force3 = int.Parse(sensorValues[2]);
                    int force4 = int.Parse(sensorValues[3]);

                    // Calculate the control inputs with smoothing
                    float horizontalInput = (force1 - force2 ) / 2046.0f; // Normalized to range -1 to 1
                    float verticalInput = (force4 - force3) / 2046.0f; // Normalized to range -1 to 1

                    // Apply smoothing to inputs
                    targetDirection = new Vector3(horizontalInput * maxHorizontalRange, verticalInput * maxVerticalRange, 1.0f);
                    targetDirection = targetDirection.normalized;

                    // Smooth the transition of target direction
                    smoothedDirection = Vector3.Lerp(smoothedDirection, targetDirection, smoothingFactor);
                }
            }
            catch (System.Exception)
            {
                // Handle exceptions (e.g., timeout or parsing errors)
            }
        }

        // Slerp towards the smoothed direction for smooth rotation
        Quaternion targetRotation = Quaternion.LookRotation(smoothedDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move the object forward in the direction it is currently facing
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    void OnApplicationQuit()
    {
        // Close the serial port when the application quits
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
