using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    // variables
    private float timer = 0f;
    private int enemiesEliminated = 0;
    private int enemiesSpawned = 0;
    public int totalEnemies;

    // references
    public GameObject enemyPrefab;
    public PlayerMovement player;
    public Transform[] spawnPoints;
    public GameObject[] suplys;
    
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
                InstantiateSuply();
            }
        }
    }

    // função para quando contar inimigos mortos e quando todos assim estiverem este termina o jogo
    public void EnemyEliminated()
    {
        // incrementa o numero de inimigos mortos
        enemiesEliminated++;
        
        // verifica se o total de inimigos é o mesmo que os que já morreram
        if (totalEnemies == enemiesEliminated)
        {
            Time.timeScale = 0;
            Debug.Log("You Win");
            
        }
    }

    public void InstantiateSuply()
    {
        // spawn a suply to help the player
        GameObject suply = Instantiate(suplys[Random.Range(0, suplys.Length)],
            new Vector3(spawnPoints[Random.Range(0,spawnPoints.Length)].transform.position.x, 3f, spawnPoints[Random.Range(0,spawnPoints.Length)].transform.position.z),
            Quaternion.identity);
        // tag
        suply.tag = "Suply"; // tag the suply

        // rotate the suply
        float rotationSpeed = 5f;
        suply.transform.Rotate(Time.deltaTime * rotationSpeed, 0, 0);
    }
}
