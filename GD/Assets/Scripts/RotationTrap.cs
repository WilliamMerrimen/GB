using UnityEngine;

public class RotationTrap : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Rigidbody rbPl = other.collider.GetComponent<Rigidbody>();
            rbPl.constraints = RigidbodyConstraints.FreezeAll; 
            Animator animPl = other.collider.GetComponent<Animator>();
            animPl.Play("Mutant Dying");
            GameOver.gameOverOn();
            GameObject map = other.collider.GetComponent<PlayerController>().mapLvl;
            Destroy(map);
        }
    }

}