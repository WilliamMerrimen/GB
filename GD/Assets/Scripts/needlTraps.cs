using UnityEngine;

public class needlTraps : sceneControl 
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            restartScene();
        }
    }

}