using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    // VARIABLES
    private float _speed = 7f;
    private float _health = 100f;
    private float _currentHealth;
    private float _damage = 20f;
    private float _timerToAttack = 0f;
    private float _attackDistance = 50f;
    private bool _idle;

    private DTNode _tree;

    private Action _rangeAttack;
    private Action _closeAttack;
    private Action _follow;

    private Func<bool> _enemyDistance;
    private Func<bool> _attackType;

    // REFERENCES
    [HideInInspector] public SpawnSystem spawnSystem;
    [HideInInspector] public Transform enemyTransform;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private HealthBar healthBar;

    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rigidbody;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _speed;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;

        _currentHealth = _health;
        healthBar.SetMaxHealth(_health);


        _rangeAttack = RangeAttack;
        _closeAttack = CloseAttack;
        _follow = Follow;

        _enemyDistance = EnemyDistance;
        _attackType = AttackType;

        DTNode dtRangeAttack = new DTAction("Range Attack", _rangeAttack);
        DTNode dtCloseAttack = new DTAction("Close Attack", _closeAttack);
        DTNode dtFollow = new DTAction("Follow Player", _follow);

        DTNode dtEnemyDistance = new DTCondition("Enemy Close", _attackType, dtCloseAttack, dtRangeAttack);

        _tree = new DTCondition("Decisions...", _enemyDistance, dtEnemyDistance, dtFollow);

    }

    // Update is called once per frame
    void Update()
    {
        _tree.Run();
        _animator.SetBool("Idle", _idle);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        healthBar.SetHealth(_currentHealth);
        if (_currentHealth <= 0f)
        {
            Die();
        }
    }
    
    void Die()
    {
        Destroy(gameObject);
        spawnSystem.BossEliminated(true);
    }

    void LookAtPlayer()
    {
        _idle = true;
        transform.LookAt(enemyTransform.transform.position);
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
        _idle = false;
        _navMeshAgent.destination = enemyTransform.position;
        transform.LookAt(enemyTransform.transform.position);
        _navMeshAgent.stoppingDistance = 10f;
        
    }

    void CloseAttack()
    {
        LookAtPlayer();
        if (_timerToAttack > 1.0f)
        {
            _timerToAttack = 0f;

            RaycastHit raycastHit;

            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z),
                transform.forward, out raycastHit, _attackDistance))
            {
                if (raycastHit.transform.CompareTag("Player"))
                {
                    PlayerMovement player = raycastHit.transform.GetComponent<PlayerMovement>();
                    player.TakeDamage(_damage);
                    muzzleFlash.Play();
                }
            }
        }
        else
            _timerToAttack += Time.deltaTime;
    }
    
    void RangeAttack()
    {
        LookAtPlayer();
        if (_timerToAttack > 2.0f)
        {
            _timerToAttack = 0f;

            RaycastHit raycastHit;

            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z),
                transform.forward, out raycastHit, _attackDistance))
            {
                if (raycastHit.transform.CompareTag("Player"))
                {
                    PlayerMovement player = raycastHit.transform.GetComponent<PlayerMovement>();
                    player.TakeDamage(_damage);
                    muzzleFlash.Play();
                }
            }
        }
        else
            _timerToAttack += Time.deltaTime;
    }
}
