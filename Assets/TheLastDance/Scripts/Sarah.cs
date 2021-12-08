using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sarah : MonoBehaviour
{
    // VARIABLES
    private float speed = 5f;
    private bool idle;
    private bool catched;
    
    // REFERENCES
    private PickUpSarah pic;
    private Animator _animator;
    private PlayerMovement player;
    private NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>();
        pic = player.GetComponent<PickUpSarah>();
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pic.pickedUp == false)
        {
            catched = false;
            idle = true;
        }
        else
        {
            _agent.SetDestination(player.transform.position);
            _agent.stoppingDistance = 5f;
            catched = true;
            idle = player.idle;
        }
        
        _animator.SetBool("Catch", catched);
        _animator.SetBool("Idle", idle);
    }
}
