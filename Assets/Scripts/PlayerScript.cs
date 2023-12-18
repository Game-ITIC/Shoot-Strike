using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 4.0f;
    public float playerSprint = 3f;
    


    [Header("Player  Health Things")]
    private float playerHealth = 120f;
    private float presentHealth;
    public HealthBar healthBar;
    /*public AudioClip playerHurtSound;
    public AudioSource audioSource;*/

    [Header("Player Script Cameras")]
    public Transform playerCamera;
    public GameObject deathCamera;
    public GameObject endGameMenuUI;

    [Header("Player Animator and Gravity")]
    public CharacterController characterController;
    public float gravity = -9.81f;
    public Animator animator;

    

    [Header("Player Jumping and velocity")]
    public float jumpRange = 1f;
    Vector3 velocity;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    public Transform surfaceCheck;
    bool onSurFace;
    public float surfaceDistance = 0.5f;
    public LayerMask surfaceMask;

    

    void Start()
    {
       
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        healthBar.GiveFullHealth(playerHealth);
    }

   
    void Update()
    {

        onSurFace = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);
        if(onSurFace && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float vertical_axis = Input.GetAxisRaw("Vertical");
        float horizontal_axis = Input.GetAxisRaw("Horizontal");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
        float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
        transform.rotation = Quaternion.Euler(0f, angel, 0f);
/*
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        characterController.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);*/

        //gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);


        PlayerMove();

        //Jump();


        //Sit();

        //Back();


        //Sprint();
    }

    void PlayerMove()
    {
        float vertical_axis = Input.GetAxisRaw("Vertical");
        float horizontal_axis = Input.GetAxisRaw("Horizontal");

        
        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if(direction.magnitude >= 0.1f)
        {

            animator.SetBool("Walk", true);
            animator.SetBool("Running", false);
            animator.SetBool("Idle", false);
            animator.SetTrigger("Jump");
            animator.SetBool("AimWalk", false);
            animator.SetBool("IdleAim", false);


            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angel, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);

        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetTrigger("Jump");
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
            animator.SetBool("AimWalk", false);
        }

    }


    /*void Jump()
    {
        if(Input.GetButtonDown("Jump") && onSurFace)
        {
            animator.SetBool("Walk", false);
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            animator.ResetTrigger("Jump");
        }
    }*/

    /*void Sprint()
    {
        if (Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurFace)
        {
            
            float vertical_axis = Input.GetAxisRaw("Vertical");
            float horizontal_axis = Input.GetAxisRaw("Horizontal");


            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Running", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                animator.SetBool("IdleAim", false);
                

                float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angel, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move(moveDirection.normalized * playerSprint * Time.deltaTime);

            }
            else
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                
            }
        }
        

    }*/

    /*public void Back()
    {
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("Back", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", false);
        }
        else
        {
            animator.SetBool("Back", false);
            
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
        }



    }*/
    /*public void Sit()
    {
        if (Input.GetKey(KeyCode.C))
        {
            
            animator.SetBool("Sit", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", false);
            
        }
       
        else
        {
           
            animator.SetBool("Sit", false);
            animator.SetBool("SitWalk", false);
            animator.SetBool("Idle", true);
           // animator.SetBool("Walk", false);
        }
    }
*/


    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);
        //audioSource.PlayOneShot(playerHurtSound);

        if (presentHealth <= 0)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        endGameMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        deathCamera.SetActive(true);
        Object.Destroy(gameObject, 1.0f);
    }

}
