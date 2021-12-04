using System;
using System.Collections;
using System.Collections.Generic;
using TheLastDance.Tutorial.Tutorial_Level;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCanvas : MonoBehaviour
{
    // VARIABLES
    private EnemyTutorial enemy;
    private BossTutorial boss;
    private Supply[] supplys;
    private Weapon[] weapons;
    private DoorTutorial door;
    
    //REFERENCES
    public GameObject enemyTMP;
    public GameObject weaponsTMP;
    public GameObject supplysTMP;
    public GameObject bossTMP;
    
    public GameObject background;
    private void Start()
    {
        background.SetActive(false);

        enemyTMP.SetActive(false);
        weaponsTMP.SetActive(false);
        supplysTMP.SetActive(false);
        bossTMP.SetActive(false);
    }

    private void Update()
    {
        enemy = FindObjectOfType<EnemyTutorial>();
        supplys = FindObjectsOfType<Supply>();
        weapons = FindObjectsOfType<Weapon>();
        boss = FindObjectOfType<BossTutorial>();
        door = FindObjectOfType<DoorTutorial>();
        
        if(enemy.trigger)
            EnemyText();
        else if(supplys[0].trigger || supplys[1].trigger)
            SupplyText();
        else if(weapons[0].trigger || weapons[1].trigger)
            WeaponsText();
        else if(boss.trigger)
            BossText();
        else if (door.trigger)
            Exit();
    }

    private void Exit()
    {
        SceneManager.LoadScene("StartMenu");
    }


    void EnemyText()
    {
        background.SetActive(true);
        enemyTMP.SetActive(true);
        Time.timeScale = 0;

        if (Input.GetKey(KeyCode.Backspace))
        {
            enemy.trigger = false;
            enemyTMP.SetActive(false);
            background.SetActive(false);
            Time.timeScale = 1;
        }
    }
    
    void BossText()
    {
        background.SetActive(true);
        bossTMP.SetActive(true);
        Time.timeScale = 0;

        if (Input.GetKey(KeyCode.Backspace))
        {
            boss.trigger = false;
            bossTMP.SetActive(false);
            background.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void WeaponsText()
    {
        background.SetActive(true);
        weaponsTMP.SetActive(true);
        Time.timeScale = 0;

        if (Input.GetKey(KeyCode.Backspace))
        {
            weapons[0].trigger = false;
            weapons[1].trigger = false;
            weaponsTMP.SetActive(false);
            background.SetActive(false);
            Time.timeScale = 1;
        }
    }
    
    void SupplyText()
    {
        background.SetActive(true);
        supplysTMP.SetActive(true);
        Time.timeScale = 0;

        if (Input.GetKey(KeyCode.Backspace))
        {
            supplys[0].trigger = false;
            supplys[1].trigger = false;
            supplysTMP.SetActive(false);
            background.SetActive(false);
            Time.timeScale = 1;
        }
    }
    
    
}
