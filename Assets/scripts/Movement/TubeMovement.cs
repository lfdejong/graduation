using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector3 moveDirection = Vector3.zero;
    [SerializeField] private CharacterController controller;

    //rotation
    private float initialYAngle = 0f;
    private float appliedGyroYAngle = 0f;
    private float calibrationYAngle = 0f;
    private Transform rawGyroRotation;
    private float tempSmoothing;

    //settings

    private float smoothing = 0.1f;

    private void Update()
    {
        //movement
     
        moveDirection = new Vector3(Input.acceleration.x * speed * Time.deltaTime, Input.acceleration.z * speed * Time.deltaTime, speed * Time.deltaTime);
        Vector3 rotMove = transform.TransformDirection(moveDirection);
        //might change accelation x to 10 or less
        controller.Move(rotMove);

        //rotation
        ApplyGyroRotation();
        ApplyCalibration();

        transform.rotation = Quaternion.Slerp(transform.rotation, rawGyroRotation.rotation, smoothing);

    }

    private IEnumerator Start()
    {
        Input.gyro.enabled = true;
        Application.targetFrameRate = 60;
        initialYAngle = transform.eulerAngles.y;

        rawGyroRotation = new GameObject("GyroRaw").transform;
        rawGyroRotation.position = transform.position;
        rawGyroRotation.rotation = transform.rotation;

        yield return new WaitForSeconds(1);

        StartCoroutine(CalibrateYAngle());
    }

    private IEnumerator CalibrateYAngle()
    {
        tempSmoothing = smoothing;
        smoothing = 1;
        calibrationYAngle = appliedGyroYAngle - initialYAngle;
        yield return null;
        smoothing = tempSmoothing;
    }

    private void ApplyGyroRotation()
    {
        rawGyroRotation.rotation = Input.gyro.attitude;
        rawGyroRotation.Rotate(0f, 0f, 180f, Space.Self);
        rawGyroRotation.Rotate(90f, 180f, 0f, Space.World);
        appliedGyroYAngle = rawGyroRotation.eulerAngles.y;
    }

    private void ApplyCalibration()
    {
        rawGyroRotation.Rotate(0f, -calibrationYAngle, 0f, Space.World);
    }

    private void SetEnabled(bool value)
    {
        enabled = true;
        StartCoroutine(CalibrateYAngle());
    }
}
