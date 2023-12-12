using UnityEngine;

public class RotationTrap : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            GameOver.gameOverOn();
        }
    }
}
