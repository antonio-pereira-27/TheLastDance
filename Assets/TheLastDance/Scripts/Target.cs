using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Target : MonoBehaviour
{
   // variáveis
   public float speed = 7f;
   public float attackDistance = 7f;
   public float health = 100f;
   private float nextAttackTime = 0f;
   public float attackRate = 0.5f;
   public float currentHealth;
   private float damage = 20f;

   public HealthBar healthBar;

   public Transform enemyTransform;

   public Transform firePoint;

   private NavMeshAgent _agent;

   private Rigidbody _rigidbody;

   public PlayerMovement _player;
   public ParticleSystem muzzleFlash;

   
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
      if (_agent.remainingDistance - attackDistance < 5f) 
      {
         if (Time.time > nextAttackTime && currentHealth < 100)
         {
            nextAttackTime = Time.time + attackRate;

            // attack
            RaycastHit hit;

            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackDistance))
            {
               muzzleFlash.Play();
               _player.TakeDamage(damage);
            }
               

            
            // mover em direção ao jogador
            _agent.destination = enemyTransform.position;
         
            // olhar sempre para ele
            transform.LookAt(enemyTransform.transform.position);
      
            // reduzir a velocidade do npc gradualmente
            _rigidbody.velocity *= 0.99f;
         }
         else
            _agent.SetDestination(RandomNavmeshLocation(10f));
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
   }
   
   public Vector3 RandomNavmeshLocation(float radius) {
      Vector3 randomDirection = Random.insideUnitSphere * radius;
      randomDirection += transform.position;
      NavMeshHit hit;
      Vector3 finalPosition = Vector3.zero;
      if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
         finalPosition = hit.position;            
      }
      return finalPosition;
   }
}
