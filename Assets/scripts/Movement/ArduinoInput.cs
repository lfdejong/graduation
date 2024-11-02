using System;
using System.IO.Ports;
using System.Linq;
using UnityEngine;

public class ArduinoInput : MonoBehaviour
{
    public string portName = "COM3";             // Serial port for reading the sensor values
    public int[] sensorValues = new int[4];      // Array to store values from 4 force sensors
    public int baudRate = 9600;                  // Standard Baudrate

    private SerialPort serialPort;
  
    
    void Start()
    {
        // Initialize serial port communication
        serialPort = new SerialPort(portName, 9600);
        serialPort.Open();
    }

    void Update()
    {
        // Read force sensor values from the serial port
        if (serialPort != null && serialPort.IsOpen)
            try
            {
                string data = serialPort.ReadLine();

                string[] values = data.Split(',');

                if (values.Length == 4)
                {
                    sensorValues[0] = int.Parse(values[1]); // Sensor 1 (Up)
                    sensorValues[1] = int.Parse(values[0]); // Sensor 2 (Down)
                    sensorValues[2] = int.Parse(values[2]); // Sensor 3 (Left)
                    sensorValues[3] = int.Parse(values[3]); // Sensor 4 (Right)
                } 
                
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Error reading from serial port: " + ex.Message);
            }
        else
        {
            // Try to reconnect when the port is not open
            //TryReconnect();
            return;
        }
    }

    private void OpenSerialPort()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();
            Debug.Log("Serial port opened " + portName);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Serial port cannot be opened: " + ex.Message);
            TryReconnect();

        }
    }
    private void TryReconnect()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }

        // Check if the COM port is available
        if (System.IO.Ports.SerialPort.GetPortNames().Contains(portName))
        {
            OpenSerialPort();
        }
        else
        {
            Debug.Log("Port " + portName + " is not available.");
            TryReconnect();
        }
    }

    private void OnApplicationQuit()
    {
        // Close the serial port when the application quits
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    // Helper methods to get the sensor values
    public int GetForceUp() { return sensorValues[0]; }   // Sensor 1 (Up)
    public int GetForceDown() { return sensorValues[1]; } // Sensor 2 (Down)
    public int GetForceLeft() { return sensorValues[2]; } // Sensor 3 (Left)
    public int GetForceRight() { return sensorValues[3]; }// Sensor 4 (Right)
}
