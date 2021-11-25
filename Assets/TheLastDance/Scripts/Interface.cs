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
    public GameObject glock;
    public GameObject ak;
    //public Gun gun;
    
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

       /* if (gun.isActiveAndEnabled)
        {
            bulletsNumber = gun.bulletsNumber.ToString();
            maxBulletsNumber = gun.maxBullets.ToString();
            
            bullets.text = "Bullets: " + bulletsNumber + "/" + maxBulletsNumber;
        }*/

    }
}
