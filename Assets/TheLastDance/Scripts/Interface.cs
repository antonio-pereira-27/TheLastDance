using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public Text bullets;

    public Text life;

    public string bulletsNumber;
    public string maxBulletsNumber;
    public string lifeText;

    public HealthBar healthBar;

    public Pistol pistol;
    public Gun gun;
    
    // Update is called once per frame
    void Update()
    {
        lifeText = healthBar.slider.value.ToString();
        life.text = lifeText;

        if (pistol.isActiveAndEnabled)
        {
            bulletsNumber = pistol.bulletsNumber.ToString();
            maxBulletsNumber = pistol.maxBullets.ToString();

            bullets.text = "Bullets: " + bulletsNumber + "/" + maxBulletsNumber;
        }

        if (gun.isActiveAndEnabled)
        {
            bulletsNumber = gun.bulletsNumber.ToString();
            maxBulletsNumber = gun.maxBullets.ToString();
            
            bullets.text = "Bullets: " + bulletsNumber + "/" + maxBulletsNumber;
        }

    }
}
