using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYPEPOLY_Rotator : MonoBehaviour
{
    public Vector3 rotationSpeed;

    void FixedUpdate()
    {
        transform.localEulerAngles += rotationSpeed;
    }
}
