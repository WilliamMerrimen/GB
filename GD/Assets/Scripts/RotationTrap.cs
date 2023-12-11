using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTrap : sceneControl
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            restartScene();
            Debug.Log("You death");
        }
    }
}
