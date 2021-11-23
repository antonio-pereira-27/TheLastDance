using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    // VARIABLES
    private float speed = 7f;
    private float health = 100f;
    private float currentHealth;
    private float damage = 20f;
    private float timerToAttack = 0f;
    private float attackDistance = 50f;

    private DTNode tree;

    private Action rangeAttack;
    private Action closeAttack;
    private Action follow;

    private Func<bool> enemyDistance;
    private Func<bool> attackType;

    // REFERENCES
    [HideInInspector] public SpawnSystem spawnSystem;
    [HideInInspector] public Transform enemyTransform;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private HealthBar healthBar;

    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;

        currentHealth = health;
        healthBar.SetMaxHealth(health);


        rangeAttack = RangeAttack;
        closeAttack = CloseAttack;
        follow = Follow;

        enemyDistance = EnemyDistance;
        attackType = AttackType;

        DTNode dtRangeAttack = new DTAction("Range Attack", rangeAttack);
        DTNode dtCloseAttack = new DTAction("Close Attack", closeAttack);
        DTNode dtFollow = new DTAction("Follow Player", follow);

        DTNode dtEnemyDistance = new DTCondition("Enemy Close", attackType, dtCloseAttack, dtRangeAttack);

        tree = new DTCondition("Decisions...", enemyDistance, dtEnemyDistance, dtFollow);

    }

    // Update is called once per frame
    void Update()
    {
        tree.Run();
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
        spawnSystem.BossEliminated();
    }

    
    // CONDITIONS
    
    private bool EnemyDistance()
    {
        float warningDistance = 30f;
        Vector3 currentDistance = enemyTransform.position - transform.position;
        float distance = currentDistance.magnitude;

        return distance < warningDistance;
    }

    private bool AttackType()
    {
        float warningDistance = 15f;
        Vector3 currentDistance = enemyTransform.position - transform.position;
        float distance = currentDistance.magnitude;

        return distance < warningDistance;
    }
    
    // ACTIONS

    private void Follow()
    {
        navMeshAgent.destination = enemyTransform.position;
        transform.LookAt(enemyTransform.transform.position);
        navMeshAgent.stoppingDistance = attackDistance / 2f;
    }

    void CloseAttack()
    {
        Follow();
        if (timerToAttack > 1.0f)
        {
            timerToAttack = 0f;

            RaycastHit raycastHit;

            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z),
                transform.forward, out raycastHit, attackDistance))
            {
                if (raycastHit.transform.CompareTag("Player"))
                {
                    PlayerMovement player = raycastHit.transform.GetComponent<PlayerMovement>();
                    player.TakeDamage(damage);
                    muzzleFlash.Play();
                }
            }
        }
        else
            timerToAttack += Time.deltaTime;
    }
    
    void RangeAttack()
    {
        Follow();
        if (timerToAttack > 2.0f)
        {
            timerToAttack = 0f;

            RaycastHit raycastHit;

            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z),
                transform.forward, out raycastHit, attackDistance))
            {
                if (raycastHit.transform.CompareTag("Player"))
                {
                    PlayerMovement player = raycastHit.transform.GetComponent<PlayerMovement>();
                    player.TakeDamage(damage);
                    muzzleFlash.Play();
                }
            }
        }
        else
            timerToAttack += Time.deltaTime;
    }
}
