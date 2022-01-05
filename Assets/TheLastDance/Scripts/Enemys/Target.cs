using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using RandomSystem = System.Random;

public class Target : MonoBehaviour
{
   // VARIABLES
   private float _speed = 5f;
   private float _attackDistance = 50f;
   private float _health = 100f;
   private float _currentHealth;
   private float _damage = 10f;
   private float _timer = 50f;
   private float _timerToAttack = 0f;
   private bool _idle;
   private float deadTimer = 0f;
   private bool dead;

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
   private AudioManager _audioManager;


   private void Start()
   {
      _audioManager = FindObjectOfType<AudioManager>();
      
      // inicializar a navmeshagent no script e atribuir valores
      _agent = GetComponent<NavMeshAgent>();
      _agent.speed = _speed;
      
      // inicializar o rigidbody através de script
      _rigidbody = GetComponent<Rigidbody>();
      _rigidbody.useGravity = false;
      
      // atualizar a barra de vida do npc
      _currentHealth = _health;
      healthBar.SetMaxHealth(_health);
      
      // animator
      animator.GetComponent<Animator>();

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
      if (!dead)
      {
         dead = false;
         _tree.Run();
         animator.SetBool("Idle", _idle);
      }

      if (dead)
      {
         animator.SetBool("Dead", true);
         if (deadTimer > 5f)
         {
            Destroy(gameObject);
            deadTimer = 0f;
         }
         else
            deadTimer++;
      }
   }

   // função para perder vida quando o jogador dispara sobre este
   public void TakeDamage(float amount)
   {
      _currentHealth -= amount;
      healthBar.SetHealth(_currentHealth);
      if (_currentHealth <= 0f) // verifica se morreu
      {
         Die();
      }
   }

   // função para destruir o gameObject
   void Die()
   {
      dead = true;
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
      
      return currentDistance < warning || _currentHealth < _health;
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
      if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out hitSearching, _attackDistance))
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
      _agent.stoppingDistance = 7f;
      if (_timer > 4f)
      {
         _idle = false;
         RandomDirection(10f);
         _timer = 0f;
      }
      else
      {
         _idle = true;
         _timer += Time.deltaTime; // aumenta o contador
      }
   }

   /*
    * ********** FOLLOW **********
    */
   void Follow()
   {
      _idle = false;
      transform.LookAt(enemyTransform.transform.position);
      _agent.destination = enemyTransform.position;
      _agent.stoppingDistance = 20f;
   }
   
   void lookAtPlayer()
   {
      transform.LookAt(enemyTransform.transform.position);
      _idle = true;
   }
   
   /*
    * ********** ATTACKS **********
    */
   void CloseAttack()
   {
      lookAtPlayer();
      // contador para atacar o jogador
      if (_timerToAttack > 1.5f)
      {
         // reiniciar o contador
         _timerToAttack = 0f;
            
         //novamente sistema de raycast do unity para acertar no jogador
         RaycastHit hitAttack;
            
         //Debug.DrawRay(transform.position, transform.forward * 7, Color.green, 1f);
         if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out hitAttack, _attackDistance))
         {
            // verifica se é o jogador através da tag
            if (hitAttack.transform.CompareTag("Player"))
            {
               // verifica a componente de jogar e tira lhe vida
               PlayerMovement player = hitAttack.transform.GetComponent<PlayerMovement>();
               player.TakeDamage(_damage);
               
               _audioManager.Play("Bullet");
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
      lookAtPlayer();
      transform.LookAt(enemyTransform.transform.position);
      // contador para atacar o jogador
      if (_timerToAttack > 3.0f)
      {
         // reiniciar o contador
         _timerToAttack = 0f;
            
         //novamente sistema de raycast do unity para acertar no jogador
         RaycastHit hitAttack;
            
         //Debug.DrawRay(transform.position, transform.forward * 7, Color.green, 1f);
         if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out hitAttack, _attackDistance))
         {
            // verifica se é o jogador através da tag
            if (hitAttack.transform.CompareTag("Player"))
            {
               // verifica a componente de jogar e tira lhe vida
               PlayerMovement player = hitAttack.transform.GetComponent<PlayerMovement>();
               player.TakeDamage(_damage);
               _audioManager.Play("Bullet");
               // inicia o sistema de particulas
               muzzleFlash.Play();
            }
         }
      }
      else
         _timerToAttack += Time.deltaTime; // aumenta o contador
   }
   
   
   

   
   
}
