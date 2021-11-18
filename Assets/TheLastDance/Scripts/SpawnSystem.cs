using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSystem : MonoBehaviour
{
    // variables
    //private float timer = 0f;
    private int _enemiesEliminated = 0;
    private int _enemiesSpawned = 0;
    private int totalEnemies;

    private int totalSuplys;
    private int _suplysSpawned = 0;

    // references
    public GameObject enemyPrefab;
    public PlayerMovement player;
    public Transform[] spawnPoints;
    public GameObject[] suplys;

    private void Start()
    {
        totalEnemies = spawnPoints.Length;
        _suplysSpawned = totalEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        // verify if all enemies were spawned
        if (_enemiesSpawned < totalEnemies)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                // point to spawn an enemy
                Transform spawnPoint = spawnPoints[i];
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity); // instantiate enemy
                Target enemyAi = enemy.GetComponent<Target>(); // get the target component
                
                // give values to variables on enemy
                enemyAi.enemyTransform = player.transform;
                enemyAi.spawnSystem = this;
                
                // count the instantiated enemies
                _enemiesSpawned++;

                
                InstantiateSuply(spawnPoint.position);
                
            }
        }
    }

    // Function that verify how many enemies were eliminated and if they are all dead finish the game
    public void EnemyEliminated()
    {
        // count the dead enemies
        _enemiesEliminated++;
        
        // verify the amount of enemies
        if (totalEnemies == _enemiesEliminated)
        {
            Time.timeScale = 0;
            Debug.Log("You Win");
            
        }
    }

    private void InstantiateSuply(Vector3 position)
    {
        //count the instantiated supplies 
        _suplysSpawned++;
        
        // spawn a supply to help the player
        GameObject suply = Instantiate(suplys[Random.Range(0, suplys.Length)],
            new Vector3(position.x, position.y + 3f, position.z),
            Quaternion.identity);
        
        
        // rotate the suply
        float rotationSpeed = 100f;
        suply.transform.Rotate(Time.deltaTime * rotationSpeed, 0, 0);
    }
}
