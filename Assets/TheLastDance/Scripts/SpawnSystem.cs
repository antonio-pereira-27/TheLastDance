using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public GameObject enemyPrefab;

    public PlayerMovement player;

    public int totalEnemies;

    public Transform[] spawnPoints;

    private float timer = 5f;

    private int enemiesEliminated;
    private int enemiesSpawned;

    // Update is called once per frame
    void Update()
    {
        enemiesEliminated = 0;
        enemiesSpawned = 0;

        if (Time.time > timer)
        {
            timer += Time.time;

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
