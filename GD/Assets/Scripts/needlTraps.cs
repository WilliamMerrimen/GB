using UnityEngine;

public class needlTraps : MonoBehaviour 
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Animator animPl = other.GetComponent<Animator>();
            GameObject map = other.GetComponent<PlayerController>().mapLvl;
            Destroy(map);
            animPl.Play("Mutant Dying");
            GameOver.gameOverOn();
        }
    }

}