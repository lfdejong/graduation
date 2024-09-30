using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    [SerializeField] private float x = 31.99211f;
    [SerializeField] private float y = 79.12196f;
    [SerializeField] private float z = 95.99429f;

    // Start is called before the first frame update
    void Start()
    {
        //initialize starting position

        transform.position = new Vector3(x, y, z);
    }
}
