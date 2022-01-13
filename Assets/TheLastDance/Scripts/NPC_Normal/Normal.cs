using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Normal : MonoBehaviour
{
    //variables
    private float _speed = 7f;
    private float _stop = 10f;
    private float _timer = 50f;
    private bool _idle = false;

    private DTNode _tree;

    private Action _work;
    private Action _exitAction;

    private Func<bool> _closePlayer;

    // references
    private NavMeshAgent _agent;
    private Rigidbody _rigidbody;
    
    private PlayerMovement _player;

    public Animator animator;

    [HideInInspector]public Transform exit;
    
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
        _agent.stoppingDistance = _stop;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;

        _player = FindObjectOfType<PlayerMovement>();
        
        
        // ACTIONS
        _work = Working;
        _exitAction = Run;
        
        DTNode workNode = new DTAction("Work", _work);
        DTNode exitNode = new DTAction("RUN", _exitAction);
        
        // CONDITIONS
        _closePlayer = PlayerClose;

        _tree = new DTCondition("Player Close", _closePlayer, exitNode, workNode);
    }

    // Update is called once per frame
    void Update()
    {
        _tree.Run();
        animator.SetBool("idle", _idle);
    }
    
    // func Random Direction
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

    // ****** Condition *********
    private bool PlayerClose()
    {
        float warning = 20f;
        Vector3 distance = _player.transform.position - transform.position;
        float currentDistance = distance.magnitude;

        return currentDistance < warning;
    }
    
    // ********************************* ACTIONS *************************************************
    
    void Working()
    {
        if (_timer > 7f)
        {
            _idle = false;
            RandomDirection(10f);
            _timer = 0f;
        }
        else
        {
            _idle = true;
            _timer += Time.deltaTime; 
        }
    }

    void Run()
    {
        _idle = false;
        _agent.SetDestination(exit.position);

        Vector3 distance = exit.transform.position - transform.position;
        float currentDistance = distance.magnitude;

        if (currentDistance <= 7f)
            Destroy(gameObject);
    }
}
