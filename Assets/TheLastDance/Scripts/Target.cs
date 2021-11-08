using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Target : MonoBehaviour
{
   // variáveis
   public float speed = 7f;
   public float attackDistance = 7f;
   public float health = 100f;
   private float _nextAttackTime;
   public float attackRate = 0.5f;
   public float currentHealth;
   private float _damage = 10f;
   
   public float directionChangeInterval = 1;
   public float maxHeadingChange = 30;
   float heading;
   Vector3 targetRotation;

   public HealthBar healthBar;

   [HideInInspector]
   public Transform enemyTransform;

   [HideInInspector] public SpawnSystem spawnSystem;

   private NavMeshAgent _agent;

   private Rigidbody _rigidbody;
   
   public ParticleSystem muzzleFlash;

   private void Awake()
   {
      heading = Random.Range(0, 360);
      transform.eulerAngles = new Vector3(0, heading, 0);
      StartCoroutine(NewHeading());
      
   }

   // função de inicio 
   private void Start()
   {
      _agent = GetComponent<NavMeshAgent>();
      _agent.stoppingDistance = attackDistance;
      _agent.speed = speed;
      _rigidbody = GetComponent<Rigidbody>();
      _rigidbody.useGravity = false;
      
      currentHealth = health;
      healthBar.SetMaxHealth(health);

      
   }

   void Update()
   {
      /*if (_agent.remainingDistance - attackDistance < 0.01f) 
      {
         if (Time.time > _nextAttackTime )
         {
            _nextAttackTime = Time.time + attackRate;

            // attack
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, attackDistance))
            {
               if (hit.transform.CompareTag("Player"))
               {
                  muzzleFlash.Play();
                  PlayerMovement player = hit.transform.GetComponent<PlayerMovement>();
                  player.TakeDamage(_damage);
               }
            }
         }
      }
      
      // mover em direção ao jogador
      _agent.destination = enemyTransform.position;
         
      // olhar sempre para ele
      transform.LookAt(new Vector3(enemyTransform.transform.position.x, enemyTransform.position.y, enemyTransform.position.z));
      
      // reduzir a velocidade do npc gradualmente
      _rigidbody.velocity *= 0.99f;*/

      transform.eulerAngles =
         Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
      Vector3 forward = transform.TransformDirection(Vector3.forward);
      _agent.Move(forward * speed);
      
      

   }
   

   public void TakeDamage(float amount)
   {
      currentHealth -= amount;
      healthBar.SetHealth(currentHealth);
      if (currentHealth <= 0f)
      {
         Die();
      }
   }

   void Die()
   {
      Destroy(gameObject);
      spawnSystem.EnemyEliminated();
   }
   

   // calcula repetidamente uma nova direção para se mover! 
   IEnumerator NewHeading()
   {
      while (true)
      {
         NewHeadingRoutine();
         yield return new WaitForSeconds(directionChangeInterval);
      }
   }

   // calculo da nova direção para mover o npc
   void NewHeadingRoutine()
   {
      var min = transform.eulerAngles.y - maxHeadingChange;
      var max = transform.eulerAngles.y + maxHeadingChange;
      targetRotation = new Vector3(0, heading, 0);
   }
}
