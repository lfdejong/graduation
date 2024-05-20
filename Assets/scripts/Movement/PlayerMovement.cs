using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{

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
    }

    // Update is called once per frame
    void Update()
    {
        activeStrafe = Mathf.Lerp(activeStrafe, UnityEngine.Input.gyro.rotationRateUnbiased.z * strafeSpeed, accStrafe * Time.deltaTime);
        activeHover = Mathf.Lerp(activeHover, UnityEngine.Input.gyro.rotationRateUnbiased.x * hoverSpeed, accHover * Time.deltaTime);
        
        transform.position += transform.forward * forwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activeStrafe * Time.deltaTime) + (transform.up * activeHover * Time.deltaTime);

        transform.rotation = UnityEngine.Input.gyro.attitude;

        // Smoothly tilts a transform towards a target rotation.
        float tiltAroundZ = UnityEngine.Input.gyro.rotationRateUnbiased.y * tiltAngle;
        float tiltAroundX = UnityEngine.Input.gyro.rotationRateUnbiased.x * tiltAngle;

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }

}
