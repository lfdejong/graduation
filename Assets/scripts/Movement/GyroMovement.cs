using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Windows;

public class GyroMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed, strafe, hover;
    [SerializeField] private float strafeSpeed, hoverSpeed;


    private Rigidbody rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        UnityEngine.Input.gyro.enabled = true;
    }

    private void Update()
    {
        strafe = Mathf.Lerp(strafe, UnityEngine.Input.gyro.rotationRateUnbiased.z, strafeSpeed * Time.deltaTime );
        strafe = Mathf.Clamp(strafe, -02, 02);
        Debug.Log(UnityEngine.Input.gyro.attitude);

    }

    private void FixedUpdate()
    {

        transform.position = transform.position + new Vector3(strafe, 0, movementSpeed * Time.deltaTime);
        rb.AddTorque(transform.forward * strafe);
    }

}
