using UnityEngine;

public class needlTraps : MonoBehaviour 
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameOver.gameOverOn();
        }
    }

}