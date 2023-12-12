using UnityEngine;

public class needlTraps : MonoBehaviour 
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rbPl = other.GetComponent<Rigidbody>();
            rbPl.constraints = RigidbodyConstraints.FreezeAll;
            GameOver.gameOverOn();
        }
    }

}