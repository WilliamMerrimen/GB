using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneControl : MonoBehaviour
{
    public void restartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public  void mainActive() {
        SceneManager.LoadScene(0);
    }

    public  void sceneLoad( int sceneId) {
        SceneManager.LoadScene(sceneId);
    }

}