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

    private int bossEliminated = 0;
    private int bossCount = 0;
    public bool deadBoss = false;

    // references
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public PlayerMovement player;
    public Transform[] spawnPoints;
    public GameObject[] suplys;
    public Transform bossSpawnPoint;

    private void Start()
    {
        totalEnemies = spawnPoints.Length;
    }

    // Update is called once per frame
    void Update()
    {
        // verify if all enemies were spawned
        if (_enemiesSpawned < totalEnemies)
        {
            InstantiateEnemys();
        }

        if (_enemiesEliminated == _enemiesSpawned)
        {
            InstantiateBoss();
        }

        if (bossEliminated == 1)
            deadBoss = true;
        
    }

    // Function that verify how many enemies were eliminated and if they are all dead finish the game
    public void EnemyEliminated()
    {
        // count the dead enemies
        _enemiesEliminated++;
        
    }

    public void BossEliminated()
    {
        bossEliminated++;
    }

    private void InstantiateBoss()
    {
        while (bossCount < 1)
        {
            GameObject bossGO = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            Boss boss = bossGO.GetComponent<Boss>();

            boss.spawnSystem = this;
            boss.enemyTransform = player.transform;

            bossCount++;
        }
    }

    private void InstantiateEnemys()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // point to spawn an enemy
            Transform spawnPoint = spawnPoints[i];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity); // instantiate enemy
            Target enemyAi = enemy.GetComponent<Target>(); // get the target component
                
            enemyAi.spawnSystem = this;
            enemyAi.enemyTransform = player.transform;
                
            // count the instantiated enemies
            _enemiesSpawned++;
                
            InstantiateSuply(spawnPoint.position);
        }
    }

    private void InstantiateSuply(Vector3 position)
    {

        // spawn a supply to help the player
        GameObject suply = Instantiate(suplys[Random.Range(0, suplys.Length)],
            new Vector3(position.x, position.y + 3f, position.z),
            Quaternion.identity);
        
        // rotate the suply
        float rotationSpeed = 100f;
        suply.transform.Rotate(Time.deltaTime * rotationSpeed, 0, 0);
    }
}
