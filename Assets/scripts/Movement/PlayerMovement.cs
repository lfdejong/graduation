using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    //player input
    private PlayerControls playerControls;

    //floats for flying
    [SerializeField] private float forwardSpeed = 25f, strafeSpeed = 7.5f, hoverSpeed = 5f;
    private float activeStrafe, activeHover;
    private float accStrafe = 2f, accHover = 2f;

    private Vector3 moveInput = Vector3.zero;

    //turning
    float smooth = 5.0f;
    float tiltAngle = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Input.gyro.enabled = true;
        playerControls = new PlayerControls();
    }

    // Update is called once per frame
    void Update()
    {
        activeStrafe = Mathf.Lerp(activeStrafe, moveInput.x * strafeSpeed, accStrafe * Time.deltaTime);
        activeHover = Mathf.Lerp(activeHover, moveInput.y * hoverSpeed, accHover * Time.deltaTime);

        transform.position += transform.forward * forwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activeStrafe * Time.deltaTime) + (transform.up * activeHover * Time.deltaTime);

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

        moveInput = new Vector3(inputVec.x, inputVec.y, 0);
    }
}
