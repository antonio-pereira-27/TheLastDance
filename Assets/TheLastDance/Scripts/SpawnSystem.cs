using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    // variáveis
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
        // diminuir o timer
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            // reiniciar o contador de tempo
            timer = 10f;
            
            // verifica se os todos os inimigos ja foram instanciados
            if (enemiesSpawned < totalEnemies)
            {
                // definir o ponto para instanciar os inimigos
                Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity); // instanciar o inimigo
                Target enemyAi = enemy.GetComponent<Target>(); // pegar na componente target
                // atribuir valores as variáveis da componente
                enemyAi.enemyTransform = player.transform;
                enemyAi.spawnSystem = this;
                // contar os inimigos que ja foram instanciados
                enemiesSpawned++;
            }
        }
    }

    // função para quando contar inimigos mortos e quando todos assim estiverem este termina o jogo
    public void EnemyEliminated()
    {
        enemiesEliminated++;

        // verifica se o total de inimigos é o mesmo que os que já morreram
        if (totalEnemies == enemiesEliminated)
        {
            Time.timeScale = 0;
            Debug.Log("You Win");
            
        }
    }
}
