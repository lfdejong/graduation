using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RingActivation : MonoBehaviour
{

    [SerializeField] GameObject ringCollission;

    private float rotateSpeed = 30f;

    private void Start()
    {
        ringCollission = GetComponent<GameObject>();
    }

    private void Update()
    {
        //transform.rotation = UnityEngine.Quaternion.Euler( 0, 0, rotateSpeed * Time.deltaTime) * transform.rotation;
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
                Debug.Log("Hit");
                StartCoroutine(Fade());
               // Destroy(gameObject);
        }
        else
        {
            return;
        }
        
    }

    IEnumerator Fade()
    {
        rotateSpeed = 60f;
        yield return new WaitForSeconds(5);
        
    }


}
