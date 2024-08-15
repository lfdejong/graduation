using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // The player's transform
    public Transform cameraTarget; // The camera's target (usually an empty GameObject)
    public float distance; // Distance from the player
    public float height; // Height above the player
    public float heightDamping = 2.0f; // Damping for height adjustment
    public float rotationDamping = 3.0f; // Damping for rotation

    void FixedUpdate()
    {
        // Check if player and cameraTarget are assigned
        if (!player || !cameraTarget)
            return;

        // Get the current position and rotation of the player
        float wantedRotationAngle = player.eulerAngles.y;
        float wantedHeight = player.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Smoothly damp the rotation angle
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Smoothly damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to distance meters behind the player
        transform.position = player.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Always look at the camera target
        transform.LookAt(cameraTarget);
    }
}
