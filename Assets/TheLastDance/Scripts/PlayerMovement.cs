using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float speed;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float slowSpeed = 5f;

    private Vector3 moveDirection;
    private Vector3 velocity;
    
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;
    
    [SerializeField] private float jumpHeight = 3f;
    private float health = 100f;
    private float currentHealth;
    
    //REFERENCES
    private CharacterController controller;
    public Transform groundCheck;
    
    public HealthBar healthBar;
    public Gun _weapon1, _weapon2;

    private Animator _animator;
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        currentHealth = health;
        healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
        // se estiverem a dar reload nao pode disparar
        if (_weapon1.isReloading || _weapon2.isReloading)
            return;
        
        // código para a arma principal
        if (_weapon1.isActiveAndEnabled)
        {
            if (_weapon1.bulletsNumber <= 0)
            {
                StartCoroutine(_weapon1.Reload());
                return;
            }
            if (Input.GetButton("Fire1") && Time.time >= _weapon1.nextTimeToFire && _weapon1.bulletsNumber > 0)
            {
                _weapon1.nextTimeToFire = Time.time + 1f / _weapon1.fireRate;
                _weapon1.Shoot();
            }
            if(Input.GetButton("Reload") && _weapon1.bulletsNumber < _weapon1.maxBullets)
            {
                StartCoroutine(_weapon1.Reload());
                return;
            }
        }
        
        
        // código para a arma secundaria
        if (_weapon2.isActiveAndEnabled)
        {
            if (_weapon2.bulletsNumber <= 0)
            {
                StartCoroutine(_weapon2.Reload());
                return;
            }

            if (Input.GetButton("Fire1") && Time.time >= _weapon2.nextTimeToFire && _weapon2.bulletsNumber > 0)
            {
                _weapon2.nextTimeToFire = Time.time + 1f / _weapon2.fireRate;
                _weapon2.Shoot();
            }

            if(Input.GetButton("Reload") && _weapon2.bulletsNumber < _weapon2.maxBullets)
            {
                StartCoroutine(_weapon2.Reload());
            }
        }
    }

    private void Move()
    {
        // update the variable isGrounded checking if we are grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        // if we are grounded stop apply gravity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }
        
        // movement 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        // calculate the moveDirection with the Inputs
        moveDirection = transform.right * x + transform.forward * z;

        if (isGrounded)
        {
            
            if (Input.GetKey(KeyCode.C))
            {
                Crouch();
            }
            else
            {
                //set the controller height to normal
                controller.height = 3.33f;
                
                // check if we are moving and if the keys are being pressed to update the velocity and the movement of character
                if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)) // run
                {
                    Run();
                }
                else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift)) // walk
                {
                    Walk();
                }
                else if (moveDirection == Vector3.zero) // Idle
                {
                    Idle();
                }
                //Jump
                if (Input.GetKey(KeyCode.Space))
                {
                    Jump();
                }
            }
            moveDirection *= speed;
        }
        
        // apply the changes calculated before
        controller.Move(moveDirection * Time.deltaTime);
        //apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    // crouch movement
    private void Crouch()
    {
        controller.height = 1.4f;
        speed = slowSpeed;
    }

    // Idle position
    private void Idle()
    {
        _animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
    }

    // walk movement
    private void Walk()
    {
        speed = slowSpeed;
        _animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    // RUn movement
    private void Run()
    {
        speed = walkSpeed;
        _animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    //take damage when its called
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0f)
        {
            Debug.Log("You die");
            Time.timeScale = 0;
        }
    }
}
