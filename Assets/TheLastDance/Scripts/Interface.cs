using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    //variables
    public string bulletsNumber;
    public string maxBulletsNumber;
    public string lifeText;

    //references
    public Text bullets;
    public Text life;
    public TMP_Text timerText;
    public HealthBar healthBar;
    public Gun pistol;
    public Gun gun;
    public GameObject glock;
    public GameObject ak;
    public GameObject pausePanel;
    public GameObject interfaceGO;

    private GameManager _gameManager;


    private void Start()
    {
        // game manager
        _gameManager = FindObjectOfType<GameManager>();
        
        //Time.timeScale = 1;
        pausePanel.SetActive(false);
        interfaceGO.SetActive(true);

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        if (gun == null)
        {
            Debug.LogWarning("No second game yet");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // health bar
        lifeText = healthBar.slider.value.ToString();
        life.text = lifeText;

        // guns
        if (pistol.isActiveAndEnabled)
        {
            glock.SetActive(true);
            ak.SetActive(false);
            bulletsNumber = pistol.bulletsNumber.ToString();
            maxBulletsNumber = pistol.maxBullets.ToString();

            bullets.text = bulletsNumber + "/" + maxBulletsNumber;
        }

        if (gun != null)
        {
            if (gun.isActiveAndEnabled)
            {
                glock.SetActive(false);
                ak.SetActive(true);
                bulletsNumber = gun.bulletsNumber.ToString();
                maxBulletsNumber = gun.maxBullets.ToString();
            
                bullets.text = bulletsNumber + "/" + maxBulletsNumber;
            }
        }

        // timer to level 3
        if (SceneManager.GetActiveScene().name == "Level_3")
        {
            timerText.gameObject.SetActive(true);
            timerText.text = string.Format("Time Left : {0:f}s", _gameManager.timer);
        }
        
        
        
        

    }
}
