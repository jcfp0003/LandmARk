using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
