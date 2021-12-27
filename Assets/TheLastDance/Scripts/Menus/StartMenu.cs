using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    //references
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void StartButton()
    {
        _audioManager.Play("Button");
        SceneManager.LoadScene("Level_1");
        
    }

    public void OptionsButton()
    {
        _audioManager.Play("Button");
        SceneManager.LoadScene("Options_Menu");
    }

    public void TutorialButton()
    {
        _audioManager.Play("Button");
        SceneManager.LoadScene("Tutorial_Level");
    }

    public void Credits()
    {
        _audioManager.Play("Button");
    }

    public void Quit()
    {
        _audioManager.Play("Button");
        Application.Quit();
    }
    
}
