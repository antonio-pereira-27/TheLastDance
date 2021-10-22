using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private float health = 100f;
    private float currentHealth;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public HealthBar healthBar;

    public Gun _weapon1, _weapon2;
    
    Vector3 velocity;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        // movimentação
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        
        // saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

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
                return;
            }
        }
        

        // gravidade 
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

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
