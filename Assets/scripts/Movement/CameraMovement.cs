using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 _offset;
    [SerializeField] private Transform player;
    [SerializeField] private float smoothTime;
    [SerializeField]  private Vector3 offset;
    [SerializeField] private float speed;
    private Vector3 currentVelocity = Vector3.zero;


    private void Awake()
    {
        offset = transform.position - player.position;
        transform.position = player.position + offset;
    }

    private void Update()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, speed);
    }
}
