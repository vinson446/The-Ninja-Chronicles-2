using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoToExtra()
    {
        SceneManager.LoadScene("Extra");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToEnd()
    {
        SceneManager.LoadScene("End");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
