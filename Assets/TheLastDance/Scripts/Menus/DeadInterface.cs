using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadInterface : MonoBehaviour
{
    //VARIABLES
    private string sceneName;
    
    //REFERENCES
    private AudioManager _audioManager;

    public GameObject _interface;
    public GameObject _pauseMenu;
    public GameObject _deadInterface;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PlayerMovement>().dead || FindObjectOfType<GameManager>().timer <= 0f)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            
            _audioManager.Play("dead");
            
            _interface.SetActive(false);
            _pauseMenu.SetActive(false);
            _deadInterface.SetActive(true);
            
           
            foreach (var sound in _audioManager.sounds)
            {
                if (sound.name == "Background")
                {
                    sound.volume = 0;
                }
            }
            
        }

      
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(sceneName);
        FindObjectOfType<PlayerMovement>().dead = false;
        Time.timeScale = 1;
    }

    public void QuitButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
