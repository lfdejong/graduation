using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 10;
    public bool timerIsRunning = true;

    [SerializeField] MovementController movementController;

    [SerializeField] private TextMeshProUGUI textMeshPro;


    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

    }
   
  
    private void Start()
    {
        //Plane's forwardForce is set to Zero.
        movementController.forwardForce = 0f;
    }

    void Update()
    {
        //If timerIsRunning is true
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
                {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
                {
                timeRemaining = 0;
                timerIsRunning = false;
                movementController.forwardForce = 50f;
                textMeshPro.enabled = false;
            }
            }
    }

    void DisplayTime(float timeRemaining)
    {
        timeRemaining += 1;
        //float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        textMeshPro.text = Mathf.RoundToInt(timeRemaining).ToString();
    }
  
}