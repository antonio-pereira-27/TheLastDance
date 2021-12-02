using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float speed;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float slowSpeed = 5f;

    private Vector3 _moveDirection;
    private Vector3 _velocity;
    
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;
    
    [SerializeField] private float jumpHeight = 3f;
    
    private float _health = 100f;
    private float _currentHealth;

    private bool _isCrounching = false;
    
    //REFERENCES
    private CharacterController _controller;
    public Transform groundCheck;
    public HealthBar healthBar;
    public Gun weapon1;
    public Gun weapon2;
    public Animator _animator;
    //private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        // _audioManager = FindObjectOfType<AudioManager>();
        _currentHealth = _health;
        healthBar.SetMaxHealth(_health);
        
        if(weapon2 == null)
            return;
        
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Crouch", _isCrounching);
        // move
        Move();
        
        // if reloading cant shoot
        if (weapon1.isReloading || weapon1.maxBullets == 0 || weapon2.isReloading || weapon2.maxBullets == 0)
            return;
        else
            Shoot();
    }

    private void Shoot()
    {
        // primary weapon
        if (weapon1.isActiveAndEnabled)
        {
            if (weapon1.maxBullets <= 0)
                return;
            else
            {
                if (weapon1.bulletsNumber <= 0)
                {
                   // _audioManager.Play("Reload");
                    StartCoroutine(weapon1.Reload());
                    return;
                }
                if (Input.GetButton("Fire1") && Time.time >= weapon1.nextTimeToFire && weapon1.bulletsNumber > 0)
                {
                   // _audioManager.Play("Bullet");
                    weapon1.nextTimeToFire = Time.time + 1f / weapon1.fireRate;
                    weapon1.Shoot();
                }
                if(Input.GetButton("Reload") && weapon1.bulletsNumber < weapon1.bulletsPerLoader)
                {
                    //_audioManager.Play("Reload");
                    StartCoroutine(weapon1.Reload());
                    return;
                }
            }
            
        }

        if (weapon2 != null)
        {
            // secundary weapon
            if (weapon2.isActiveAndEnabled)
            {
                if (weapon2.maxBullets <= 0)
                    return;
                else
                {
                    if (weapon2.bulletsNumber <= 0)
                    {
                        //_audioManager.Play("Reload");
                        StartCoroutine(weapon2.Reload());
                        return;
                    }
                    if (Input.GetButton("Fire1") && Time.time >= weapon2.nextTimeToFire && weapon2.bulletsNumber > 0)
                    {
                        //_audioManager.Play("Bullet");
                        weapon2.nextTimeToFire = Time.time + 1f / weapon2.fireRate;
                        weapon2.Shoot();
                    }
                    if(Input.GetButton("Reload") && weapon2.bulletsNumber < weapon2.bulletsPerLoader)
                    {
                        //_audioManager.Play("Reload");
                        StartCoroutine(weapon2.Reload());
                        return;
                    }
                }
            
            }
        }
        
        
    }
    private void Move()
    {
        // update the variable isGrounded checking if we are grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        // if we are grounded stop apply gravity
        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        
        // movement 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        // calculate the moveDirection with the Inputs
        _moveDirection = transform.right * x + transform.forward * z;
        
        if (isGrounded)
        {
            //crouch 
            if (Input.GetKey(KeyCode.C))
            {
                _isCrounching = true;
                Crouch();
            }
            else
            {
                //set the controller height to normal
                _controller.height = 3.33f;
                _isCrounching = false;
                
                // check if we are moving and if the keys are being pressed to update the velocity and the movement of character
                if (_moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)) // run
                {
                    Run();
                }
                else if(_moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift)) // walk
                {
                    Walk();
                }
                else if (_moveDirection == Vector3.zero) // Idle
                {
                    Idle();
                }

                //Jump
                if (Input.GetKey(KeyCode.Space))
                {
                    Jump();
                }
            }
        }
        
        _moveDirection *= speed;
        
        // apply the changes calculated before
        _controller.Move(_moveDirection * Time.deltaTime);
        //apply gravity
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

    }

    // crouch movement
    private void Crouch()
    {
        
        //controller.height = 1.4f;
        speed = slowSpeed;
    }

    // Idle position
    private void Idle()
    {
        _animator.SetFloat("Speed", 0);
    }

    // walk movement
    private void Walk()
    {
        speed = slowSpeed;
       // _audioManager.Play("FootSteps");
       _animator.SetFloat("Speed", 0.5f);
        
    }

    // RUn movement
    private void Run()
    {
        speed = walkSpeed;
        //_audioManager.Play("FootSteps");
        _animator.SetFloat("Speed", 0.5f);
        
    }

    private void Jump()
    {
        _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        
    }

    
    private void OnTriggerEnter(Collider suply)
    {
        // if bullet
        if (suply.CompareTag("Bullet"))
        {
            Destroy(suply.gameObject);
            
            if(weapon1.isActiveAndEnabled)
                weapon1.maxBullets += weapon1.bulletsPerLoader;
            
            if (weapon2 != null && weapon2.isActiveAndEnabled)
                weapon2.maxBullets += weapon2.bulletsPerLoader;
        }
        
        // if first aid kit
        if (suply.CompareTag("Suply"))
        {
            Destroy(suply.gameObject);
            _currentHealth += 20f;
            healthBar.SetHealth(_currentHealth);
        }
    }

    //take damage when its called
    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        healthBar.SetHealth(_currentHealth);
        if (_currentHealth <= 0f)
        {
            Debug.Log("You die");
            Time.timeScale = 0;
        }
    }
}
