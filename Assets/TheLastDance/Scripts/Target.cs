using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using RandomSystem = System.Random;

public class Target : MonoBehaviour
{
   // VARIABLES
   private float speed = 5f;
   private float attackDistance = 50f;
   private float health = 100f;
   private float currentHealth;
   private float _damage = 10f;
   private float _timer = 50f;
   private float _timerToAttack = 0f;

   private DTNode _tree;

   private Action _rangeAttack;
   private Action _closeAttack;
   private Action _follow;
   private Action _work;
   
   private Func<bool> _playerClose;
   private Func<bool> _distance;
   private Func<bool> _caught;
   
   // REFERENCES
   [HideInInspector] public SpawnSystem spawnSystem;
   [HideInInspector] public Transform enemyTransform;
   
   public ParticleSystem muzzleFlash;
   public Animator animator;
   public HealthBar healthBar;
   
   private NavMeshAgent _agent;
   private Rigidbody _rigidbody;


   private void Start()
   {
      // inicializar a navmeshagent no script e atribuir valores
      _agent = GetComponent<NavMeshAgent>();
      _agent.speed = speed;
      
      // inicializar o rigidbody através de script
      _rigidbody = GetComponent<Rigidbody>();
      _rigidbody.useGravity = false;
      
      // atualizar a barra de vida do npc
      currentHealth = health;
      healthBar.SetMaxHealth(health);

      // initialize actions
       _rangeAttack = RangeAttack;
       _closeAttack = CloseAttack;
       _distance = Distance;
       _follow = Follow;
       
       // initialize conditions
       _playerClose = PlayerClose;
       _work = Working;
       _caught = SawEnemy;

       // create action nodes
      DTNode work = new DTAction("Working", _work);
      DTNode follow = new DTAction("Follow", _follow);
      DTNode attack = new DTAction("Close Attack", this._closeAttack);
      DTNode shootAttack = new DTAction("Shoot Attack", _rangeAttack);
      
      // create condition nodes
      DTNode attackType = new DTCondition("Attack Type", _distance, attack, shootAttack);
      DTNode sawEnemy = new DTCondition("Saw Enemy", _caught , follow, work);

      // create condition nodes
      _tree = new DTCondition("Player Close", _playerClose, attackType, sawEnemy);
      
   }
   
   // função update que atualiza a cada frame
   void Update()
   {
      _tree.Run();
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
   
   // CONDITIONS
   /*
    * ********** FIND PLAYER **********
    */
   private bool PlayerClose()
   {
      float warning = 20f;
      Vector3 distance = enemyTransform.position - transform.position;
      float currentDistance = distance.magnitude;
      
      return currentDistance < warning || currentHealth < health;
   }
   
   
   /*
    * ********** DISTANCE **********
    */
   bool Distance()
   {
      float warning = 10f;
      Vector3 distance = enemyTransform.position - transform.position;
      float currentDistance = distance.magnitude;
      
      return currentDistance < warning;
   }
   

   bool SawEnemy()
   {
      bool caught = false;
      // procura pelo jogador através do sistema de raycast do unity
      RaycastHit hitSearching;
         
      //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), transform.forward * 7, Color.magenta, 1f);
      if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out hitSearching, attackDistance))
      {
         // verifica se é o jogador através da tag
         if (hitSearching.transform.CompareTag("Player"))
            caught = true;
      }

      return caught;
   }

   // ACTIONS
   /*
    * ********** WORK **********
    */
   
   // função para dar uma direção aleatória ao npc
   private void RandomDirection(float radius)
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
   void Working()
   {
      _agent.stoppingDistance = 5f;
      if (_timer > 4f)
      {
         RandomDirection(10f);
         _timer = 0f;
      }
      else
      {
         _timer += Time.deltaTime; // aumenta o contador
      }
   }

   /*
    * ********** FOLLOW **********
    */
   void Follow()
   {
      transform.LookAt(enemyTransform.transform.position);
      _agent.destination = enemyTransform.position;
      _agent.stoppingDistance = 20f;
   }
   
   /*
    * ********** ATTACKS **********
    */
   void CloseAttack()
   {
      Follow();
      // contador para atacar o jogador
      if (_timerToAttack > 1.5f)
      {
         // reiniciar o contador
         _timerToAttack = 0f;
            
         //novamente sistema de raycast do unity para acertar no jogador
         RaycastHit hitAttack;
            
         //Debug.DrawRay(transform.position, transform.forward * 7, Color.green, 1f);
         if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out hitAttack, attackDistance))
         {
            // verifica se é o jogador através da tag
            if (hitAttack.transform.CompareTag("Player"))
            {
               // verifica a componente de jogar e tira lhe vida
               PlayerMovement player = hitAttack.transform.GetComponent<PlayerMovement>();
               player.TakeDamage(_damage);
               // inicia o sistema de particulas
               muzzleFlash.Play();
            }
         }
      }
      else
         _timerToAttack += Time.deltaTime; // aumenta o contador
   }
   
   void RangeAttack()
   {
      Follow();
      transform.LookAt(enemyTransform.transform.position);
      // contador para atacar o jogador
      if (_timerToAttack > 3.0f)
      {
         // reiniciar o contador
         _timerToAttack = 0f;
            
         //novamente sistema de raycast do unity para acertar no jogador
         RaycastHit hitAttack;
            
         //Debug.DrawRay(transform.position, transform.forward * 7, Color.green, 1f);
         if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out hitAttack, attackDistance))
         {
            // verifica se é o jogador através da tag
            if (hitAttack.transform.CompareTag("Player"))
            {
               // verifica a componente de jogar e tira lhe vida
               PlayerMovement player = hitAttack.transform.GetComponent<PlayerMovement>();
               player.TakeDamage(_damage);
               // inicia o sistema de particulas
               muzzleFlash.Play();
            }
         }
      }
      else
         _timerToAttack += Time.deltaTime; // aumenta o contador
   }
   
   
   

   
   
}
