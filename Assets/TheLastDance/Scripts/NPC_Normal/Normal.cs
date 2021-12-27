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
    private float speed = 7f;
    private float stop = 10f;
    private float timer = 50f;
    private bool idle = false;

    private DTNode tree;

    private Action work;
    private Action exitAction;

    private Func<bool> closePlayer;

    // references
    private NavMeshAgent agent;
    private Rigidbody rigidbody;
    
    private PlayerMovement player;

    public Animator animator;

    [HideInInspector]public Transform exit;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = stop;

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;

        player = FindObjectOfType<PlayerMovement>();
        
        
        // ACTIONS
        work = Working;
        exitAction = Run;
        
        DTNode workNode = new DTAction("Work", work);
        DTNode exitNode = new DTAction("RUN", exitAction);
        
        // CONDITIONS
        closePlayer = PlayerClose;

        tree = new DTCondition("Player Close", closePlayer, exitNode, workNode);
    }

    // Update is called once per frame
    void Update()
    {
        tree.Run();
        animator.SetBool("idle", idle);
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
            agent.SetDestination(finalPosition); // destino do npc
        }
      
    }

    // ****** Condition *********
    private bool PlayerClose()
    {
        float warning = 20f;
        Vector3 distance = player.transform.position - transform.position;
        float currentDistance = distance.magnitude;

        return currentDistance < warning;
    }
    
    // ********************************* ACTIONS *************************************************
    
    void Working()
    {
        if (timer > 7f)
        {
            idle = false;
            RandomDirection(10f);
            timer = 0f;
        }
        else
        {
            idle = true;
            timer += Time.deltaTime; 
        }
    }

    void Run()
    {
        idle = false;
        agent.SetDestination(exit.position);

        Vector3 distance = exit.transform.position - transform.position;
        float currentDistance = distance.magnitude;

        if (currentDistance <= 7f)
            Destroy(gameObject);
    }
}
