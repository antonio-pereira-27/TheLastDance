using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public HealthBar healthBar;
    public Gun pistol;
    public Gun gun;
    public GameObject glock;
    public GameObject ak;
    public GameObject pausePanel;
    public GameObject interfaceGO;


    private void Start()
    {
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
        lifeText = healthBar.slider.value.ToString();
        life.text = lifeText;

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
            
                bullets.text = "Bullets: " + bulletsNumber + "/" + maxBulletsNumber;
            }
        }

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            interfaceGO.SetActive(false);
        }

    }
}
