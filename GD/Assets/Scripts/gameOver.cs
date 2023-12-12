using UnityEngine;

public static class GameOver
{
    public static GameObject _gameOverScreen;
    public static bool GameOverBl = false;
    
    public static void gameOverOn()
    {
        GameOverBl = true;
        _gameOverScreen.SetActive(true);
    }

    public static void GameOverOff()
    {
        GameOverBl = false;
        _gameOverScreen.SetActive(false);
    }

}