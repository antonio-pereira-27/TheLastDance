using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Target : MonoBehaviour
{
   // velociade
   public float speed = 7f;
   // vida
   public float health = 100f;
   public float currentHealth;
   // ataque
   private float _damage = 10f;
   public float attackDistance = 7f;
   private float timerToAttack = 0f;
   // movimentação
   private bool chasing = false;
   private float timer = 50f;
   
   // barra de vida
   public HealthBar healthBar;
   // transform do player
   [HideInInspector] public Transform enemyTransform;
   // sistema de spawn de inimigos
   [HideInInspector] public SpawnSystem spawnSystem;

   // navmeshagent para npc
   private NavMeshAgent _agent;
   // rigidbody
   private Rigidbody _rigidbody;
   // sistema de particulas para efeito de disparo
   public ParticleSystem muzzleFlash;

   // função de inicio 
   private void Start()
   {
      // inicializar a navmeshagent no script e atribuir valores
      _agent = GetComponent<NavMeshAgent>();
      _agent.stoppingDistance = attackDistance;
      _agent.speed = speed;
      // inicializar o rigidbody através de script
      _rigidbody = GetComponent<Rigidbody>();
      _rigidbody.useGravity = false;
      
      // atualizar a barra de vida do npc
      currentHealth = health;
      healthBar.SetMaxHealth(health);
      
   }

   // função update que atualiza a cada frame
   void Update()
   {
      // condição para verificar se o jogador está no campo de visão do npc ou se este o atacou pelas costas
      if (!chasing && currentHealth >= 100f)
      {
         // contador de tempo para mudanças de direção
         if (timer > 1f)
         {
            RandomDirection(10f);
            timer = 0f;
         
         }
         else
            timer += Time.deltaTime; // aumenta o contador
         
         
         // procura pelo jogador através do sistema de raycast do unity
         RaycastHit hitSearching;
         //Debug.DrawRay(transform.position, transform.forward * 7, Color.magenta, 1f);
         if (Physics.Raycast(transform.position, transform.forward, out hitSearching, attackDistance))
         {
            // verifica se é o jogador através da tag
            if (hitSearching.transform.CompareTag("Player"))
            {
               Debug.Log("I found the player");
               chasing = true; // atualizar a variável de perseguição
            }
         }
         
      }
      else
      {
         // olha para o jogador e persegue-o
         transform.LookAt(enemyTransform.transform.position);
         _agent.destination = enemyTransform.position;

         // contador para atacar o jogador
         if (timerToAttack > 1.5f)
         {
            // reiniciar o contador
            timerToAttack = 0f;
            
            //novamente sistema de raycast do unity para acertar no jogador
            RaycastHit hitAttack;
            //Debug.DrawRay(transform.position, transform.forward * 7, Color.green, 1f);
            if (Physics.Raycast(transform.position, transform.forward, out hitAttack, attackDistance))
            {
               // verifica se é o jogador através da tag
               if (hitAttack.transform.CompareTag("Player"))
               {
                  Debug.Log("Ill shoot");
                  // verifica a componente de jogar e tira lhe vida
                  PlayerMovement player = hitAttack.transform.GetComponent<PlayerMovement>();
                  player.TakeDamage(_damage);
                  // inicia o sistema de particulas
                  muzzleFlash.Play();
               }
            }
         }
         else
            timerToAttack += Time.deltaTime; // aumenta o contador
         
      }
   }
   
   // função para perder vida quando o jogador dispara sobre este
   public void TakeDamage(float amount)
   {
      currentHealth -= amount;
      healthBar.SetHealth(currentHealth);
      if (currentHealth <= 0f) // verifica se morreu
      {
         Die();
      }
   }

   // função para destruir o gameObject
   void Die()
   {
      Destroy(gameObject);
      spawnSystem.EnemyEliminated();
   }

   // função para dar uma direção aleatória ao npc
   public void RandomDirection(float radius)
   {
      // através de um angulo definido entre -pi e pi
      float angle = Random.Range(-Mathf.PI, Mathf.PI);
      // a direção que o inimigo vai seguir é feita através do seno do angulo para o eixo X e através do cosseno para o eixo Z
      Vector3 randomDirection = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius;
      // a esta direção adiciona-se a posição do npc
      randomDirection += transform.position;
      
      NavMeshHit navMeshHit;
      Vector3 finalPosition = Vector3.zero;
      // através da navmeshhit do unity é feito o ponto final para onde o npc irá seguir
      if (NavMesh.SamplePosition(randomDirection, out navMeshHit, radius, 1))
      {
         // a sua posição final
         finalPosition = navMeshHit.position;
         //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = finalPosition;
         _agent.SetDestination(finalPosition); // destino do npc
      }
      
   }
   
   
}
