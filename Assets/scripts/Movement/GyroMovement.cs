using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GyroMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 moveInput = Vector3.zero;

    float smooth = 5.0f;
    float tiltAngle = 60.0f;
    // Start is called before the first frame update

    void Awake()
    {
        //Input.gyro.enabled= true;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        
}

    private void OnEnable()
    {
        playerControls.Flying.Enable();
    }

    private void OnDisable()
    {
        playerControls.Flying.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveInput * movementSpeed * Time.deltaTime);
        transform.rotation = UnityEngine.Input.gyro.attitude;

        // Smoothly tilts a transform towards a target rotation.
        float tiltAroundZ = moveInput.x * tiltAngle;
        float tiltAroundX = moveInput.y * tiltAngle;

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }

    private void OnMove(InputValue inputValue)
    {
        Vector2 inputVec = inputValue.Get<Vector2>();

        moveInput = new Vector3(inputVec.x, inputVec.y, movementSpeed);
    }
}
