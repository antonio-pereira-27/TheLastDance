using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public GameObject enemyPrefab;

    public PlayerMovement player;

    public int totalEnemies;

    public Transform[] spawnPoints;

    private float timer = 0f;

    private int enemiesEliminated = 0;
    private int enemiesSpawned = 0;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = 20f;
            
            if (enemiesSpawned < totalEnemies)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                Target enemyAi = enemy.GetComponent<Target>();
                enemyAi.enemyTransform = player.transform;
                enemyAi.spawnSystem = this;
                enemiesSpawned++;
            }
        }
    }

    public void EnemyEliminated()
    {
        enemiesEliminated++;

        if (totalEnemies == enemiesEliminated)
        {
            Time.timeScale = 0;
            Debug.Log("You Win");
            
        }
    }
}
