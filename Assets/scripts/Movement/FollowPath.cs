using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] Transform[] points; //list of the path
    [SerializeField] private float MoveSpeed;

    private int pointIndex;

    //movement variables
    private int indexLane = 1;//0:left,1:middle,2:right
    [SerializeField]private float distanceLane;
    //rotate speed
    [SerializeField] float turnSpeed = 20f;

    // Start is called before the first frame update
    void Start()
        {
            transform.position = points[pointIndex].transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if(pointIndex <= points.Length - 1)
            {
            //following path
                transform.position = Vector3.MoveTowards(transform.position, points[pointIndex].transform.position, MoveSpeed * Time.deltaTime);
                transform.Rotate(Vector3.up * 0f * turnSpeed * Time.deltaTime);

            if (transform.position == points[pointIndex].transform.position)
                {
                    pointIndex += 1;
                }

                //moving and rotating side to side
                if(indexLane == 0)
            {
                transform.position += Vector3.left * distanceLane * Time.deltaTime;
               
            }
            else if (indexLane == 2)
            {
                transform.position += Vector3.right * distanceLane * Time.deltaTime;
               
            }
        }
        }

    public void LeftTap()
    {
        indexLane--;
        if(indexLane == -1)
        {
            indexLane = 0;
        }


    }
    public void Righttap()
    {
        indexLane++;
        if (indexLane == 3)
        {
            indexLane = 2;
        }
    }
}
