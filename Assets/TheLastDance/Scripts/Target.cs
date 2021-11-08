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
   public float attackRate = 5f;
   public float currentHealth;
   private float _damage = 10f;
   private float timer = 50f;
   private bool chasing = false;
   private float attackDuration = 0f;
   

   public HealthBar healthBar;

   [HideInInspector]
   public Transform enemyTransform;

   [HideInInspector] public SpawnSystem spawnSystem;

   private NavMeshAgent _agent;

   private Rigidbody _rigidbody;
   
   public ParticleSystem muzzleFlash;

   // função de inicio 
   private void Start()
   {
      _agent = GetComponent<NavMeshAgent>();
      //_agent.stoppingDistance = attackDistance;
      _agent.speed = speed;
      _rigidbody = GetComponent<Rigidbody>();
      _rigidbody.useGravity = false;
      
      currentHealth = health;
      healthBar.SetMaxHealth(health);

      
   }

   void Update()
   {
      if (!chasing)
      {
         if (timer > 5f)
         {
            RandomDirection(10f);
            timer = 0f;
         
         }
         else
         {
            timer += Time.deltaTime;
         }
         

         RaycastHit hitSearching;
         Debug.DrawRay(transform.position, transform.forward * 7, Color.magenta, 1f);
         if (Physics.Raycast(transform.position, transform.forward, out hitSearching, attackDistance))
         {
            if (hitSearching.transform.CompareTag("Player"))
            {
               Debug.Log("I found the player");
               chasing = true;
            }
         }
         
      }
      else
      {
         
         transform.LookAt(enemyTransform.transform.position);
         _agent.destination = enemyTransform.position;
         
         if (Time.time > _nextAttackTime)
         {
            attackDuration = 0.5f;
            _nextAttackTime = Time.time + attackRate;
         }
         
         if(attackDuration > 0f)
         {
            attackDuration -= Time.deltaTime;
            RaycastHit hitAttack;

            Debug.DrawRay(transform.position, transform.forward * 7, Color.green, 1f);
            if (Physics.Raycast(transform.position, transform.forward, out hitAttack, attackDistance))
            {
               if (hitAttack.transform.CompareTag("Player"))
               {
                  Debug.Log("Ill shoot");
                  
                  PlayerMovement player = hitAttack.transform.GetComponent<PlayerMovement>();
                  player.TakeDamage(_damage);
               
                  muzzleFlash.Play();
               }
            }


         }
         
      }
      

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

   public void RandomDirection(float radius)
   {
      float angle = Random.Range(-Mathf.PI, Mathf.PI);
      Vector3 randomDirection = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius;
      randomDirection += transform.position;
      NavMeshHit navMeshHit;
      Vector3 finalPosition = Vector3.zero;
      

      if (NavMesh.SamplePosition(randomDirection, out navMeshHit, radius, 1))
      {
         finalPosition = navMeshHit.position;
         //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = finalPosition;
         _agent.SetDestination(finalPosition);
      }
      
   }
   
   
}
