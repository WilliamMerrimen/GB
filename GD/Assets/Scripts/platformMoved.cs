using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformMoved : MonoBehaviour
{
    public bool forward = false;
    public int vector = 1;
    private void FixedUpdate()
    {
        if (forward)
        {
            if (vector == 1)
            {
                transform.position += new Vector3(4f, 0f, 0f) * Time.fixedDeltaTime;
            }
            else if (vector == 2)
            {
                transform.position -= new Vector3(4f, 0f, 0f) * Time.fixedDeltaTime;
            }
        }
        if (!forward)
        {
            if (vector == 1)
            {
                transform.position += new Vector3(0f, 0f, 4f) * Time.fixedDeltaTime;
            }
            else if (vector == 2)
            {
                transform.position -= new Vector3(0f, 0f, 4f) * Time.fixedDeltaTime;
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        
        vector = 2;
        Debug.Log("1232131223");
    }
    private void OnTriggerEnter(Collider other)
    {
        vector = 2;
        Debug.Log("1232131223");
    }
}
