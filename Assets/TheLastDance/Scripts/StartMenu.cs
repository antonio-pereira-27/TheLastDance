using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OptionsButton()
    {
        SceneManager.LoadScene("Options_Menu");
    }

    public void TutorialButton()
    {
        SceneManager.LoadScene("Tutorial_Level");
    }

    public void Credits()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
