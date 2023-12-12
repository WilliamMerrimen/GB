using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Home()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
