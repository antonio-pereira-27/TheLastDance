using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // VARIABLES
    private bool winLevel;
    [HideInInspector]public float timer;
    private PlayerMovement player;
    
    // REFERENCES
    public SpawnSystem spawnSystem;
    public GameObject weaponOlder;

    // Start is called before the first frame update
    void Start()
    {
        // timer
        timer = 120f;
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // verify win condition
        if (spawnSystem.deadBoss)
            winLevel = true;

        if (player.dead)
            Time.timeScale = 0;

        // win condition
        if (winLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            spawnSystem.BossEliminated(false);
        }

        // active switch guns
        if (SceneManager.GetActiveScene().name == "Level_3" || SceneManager.GetActiveScene().name == "Level_4")
            weaponOlder.GetComponent<SwitchGuns>().enabled = true;

        if (SceneManager.GetActiveScene().name == "Level_4")
            player.GetComponent<PickUpSarah>().enabled = true;
        else 
            player.GetComponent<PickUpSarah>().enabled = false;
        
        

        if (SceneManager.GetActiveScene().name == "Level_3")
        {
            print(timer);
            if (timer <= 0f)
                Time.timeScale = 0;
            else 
                timer -= Time.deltaTime;
        }

    }
}
