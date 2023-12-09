using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYPEPOLY_Moover : MonoBehaviour
{
    public float distance;
    [Range(0.01f,0.5f)]
    public float speed;

    Vector3 posToMoove;

    void Start()
    {
        posToMoove = transform.localPosition;
        posToMoove.x -= distance;
    }

    void FixedUpdate()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, posToMoove, speed);
        if(Vector3.Distance(transform.localPosition,posToMoove) < 0.1f)
        {
            posToMoove *= -1f;
        }
    }
}
