using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PauseMenu : MonoBehaviour
{
    // VARIABLES
    private float sensivity;
    public static bool paused = false;


    // REFERENCES
    public TMP_InputField sensivityInput;


    public GameObject pausePanel;
    public GameObject interfaceGO;

    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        // sensitivity
        sensivity = MouseLook.mouseSensivity;
        sensivityInput.text = sensivity.ToString();

        // audio
        _audioManager = FindObjectOfType<AudioManager>();

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (paused)
                Resume();
            else
                Pause();
        }
    }

    private void Pause()
    {
        _audioManager.Play("Button");
        Time.timeScale = 0;
        
        interfaceGO.SetActive(false);
        pausePanel.SetActive(true);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        
        paused = true;
    }

    private void Resume()
    {
        _audioManager.Play("Button");
        Time.timeScale = 1;
        
        interfaceGO.SetActive(true);
        pausePanel.SetActive(false);
       
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        paused = false;
    }

    public void ChangeSensivity()
    {
        sensivity = Single.Parse(sensivityInput.text);
        MouseLook.mouseSensivity = sensivity;
    }
    

    public void BackButton()
    {
        _audioManager.Play("Button");
        interfaceGO.SetActive(true);
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitButton()
    {
        _audioManager.Play("Button");
        SceneManager.LoadScene("StartMenu");
    }
    
}
