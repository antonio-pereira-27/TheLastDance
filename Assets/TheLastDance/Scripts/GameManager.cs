using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // VARIABLES
    private bool winLevel;
    
    // REFERENCES
    public SpawnSystem spawnSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnSystem.deadBoss)
            winLevel = true;

        if (winLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            spawnSystem.BossEliminated(false);
        }

    }
}
