using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSingleton
{
    private static GameSingleton instance;

    public static GameSingleton GetInstance()
    {
        if (instance == null)
        {
            instance = new GameSingleton();
        }
        return instance;
    }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}