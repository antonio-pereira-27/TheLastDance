using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TheLastDance.Tutorial
{
    public class PlayerTutorialMovement : MonoBehaviour
    {
        // VARIABLES
        private float speed;
        private float walkSpeed = 8f;
        private float runSpeed = 15f;

        private Vector3 direction;
        private Vector3 velocity;

        private bool isGrounded;
        private float groundDistance = 0.4f;
        private float gravity = -9.81f;
        [SerializeField] private LayerMask groundMask;

        private float jumpHeight = 1.5f;

        // REFERENCES
        private CharacterController controller;
        public Animator animator;


        private void Start()
        {
            controller = GetComponent<CharacterController>();
            
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            direction = transform.right * x + transform.forward * z;

            if (isGrounded)
            {
                if (Input.GetKey(KeyCode.C))
                {
                    animator.SetBool("Crouch", true);
                    speed = walkSpeed;
                }
                else
                {
                    //move
                    if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
                        Run();
                    else if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
                        Walk();
                    else if (direction == Vector3.zero)
                        Idle();

                    // jump
                    if (Input.GetKey(KeyCode.Space))
                        Jump();
   
                }
            }

            direction *= speed;
            controller.Move(direction * Time.deltaTime);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

        }

        private void Jump()
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //animator.Play("Jumping");
        }

        private void Idle()
        {
            speed = 0;
            animator.SetInteger("SpeedInt", 0);
            animator.SetBool("Crouch", false);
        }

        private void Walk()
        {
            speed = walkSpeed;
            animator.SetInteger("SpeedInt", 1);
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Crouch", false);
        }

        private void Run()
        {
            speed = runSpeed;
            animator.SetInteger("SpeedInt", 1);
            animator.SetFloat("Speed", 1f);
            animator.SetBool("Crouch", false);
        }
    }
}