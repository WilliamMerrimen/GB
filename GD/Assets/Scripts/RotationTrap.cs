using UnityEngine;

public class RotationTrap : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Rigidbody rbPl = other.collider.GetComponent<Rigidbody>();
            rbPl.constraints = RigidbodyConstraints.FreezeAll;
            GameOver.gameOverOn();
        }
    }
}
