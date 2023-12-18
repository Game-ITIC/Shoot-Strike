using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Health and Damage")]
    private float enemyHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public HealthBar healthBar;

    [Header("Enemy Things")]
    public NavMeshAgent enemyAgent;
    public Transform playerBody;
    public Transform LookPoint;
    public Camera ShootingRaycastArea;
    public LayerMask playerLayer;


    [Header("Enemy Guarding Var")]
    public GameObject[] walkPoints;
    int currentEnemyPositin = 0;
    public float enemySpeed;
    float walkingPointRadius = 2;


    [Header("Sounds and UI")]
    public AudioClip shootingSound;
    public AudioSource audioSource;


    [Header("Enemy Shooting Var")]
    public float timebteShoot;
    bool previouslyShoot;


    [Header("Enemy Animation and Spark effect")]
    public Animator animator;
    public ParticleSystem muzzleSpark;


    [Header("Enemy mood/situation")]
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInShootingRadius;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        presentHealth = enemyHealth;
        healthBar.GiveFullHealth(enemyHealth);
        playerBody = GameObject.Find("Player").transform;
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);


        if (!playerInShootingRadius && !playerInShootingRadius) Guard();
        if (playerInvisionRadius && !playerInShootingRadius) PursuePlayer();
        if (playerInvisionRadius && playerInShootingRadius) ShootPlayer();
    }

    private void Guard()
    {
        if(Vector3.Distance(walkPoints[currentEnemyPositin].transform.position, transform.position) < walkingPointRadius)
        {
            currentEnemyPositin = Random.Range(0, walkPoints.Length);
            if(currentEnemyPositin >= walkPoints.Length)
            {
                currentEnemyPositin = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentEnemyPositin].transform.position, Time.deltaTime * enemySpeed);
        //changing enemy face
        transform.LookAt(walkPoints[currentEnemyPositin].transform.position);

    }

    private void PursuePlayer()
    {
        if (enemyAgent.SetDestination(playerBody.position))
        {
            //animation
            animator.SetBool("Walk", false);
            animator.SetBool("AimRun", true);
            animator.SetBool("Shoot", false);
            animator.SetBool("AimDie", false);
            animator.SetBool("Die", false);

            //+vision and shooting radius
            visionRadius = 30;
            shootingRadius = 16;
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("AimRun", false);
            animator.SetBool("Shoot", false);
            animator.SetBool("AimDie", true);
            animator.SetBool("Die", true);
        }
    }

    private void ShootPlayer()
    {
        enemyAgent.SetDestination(transform.position);

        transform.LookAt(LookPoint);

        if (!previouslyShoot)
        {

            muzzleSpark.Play();
            audioSource.PlayOneShot(shootingSound);

            RaycastHit hit;
            if(Physics.Raycast(ShootingRaycastArea.transform.position, ShootingRaycastArea.transform.forward, out hit, shootingRadius))
            {
                Debug.Log("Shooting" + hit.transform.name);

                PlayerScript playerBody = hit.transform.GetComponent<PlayerScript>();

                if(playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage);
                }

                animator.SetBool("Walk", false);
                animator.SetBool("AimRun", false);
                animator.SetBool("Shoot", true);
                animator.SetBool("AimDie", false);
                animator.SetBool("Die", false);
            }

            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timebteShoot);
        }

    }

    private void ActiveShooting()
    {
        previouslyShoot = false;
    }


    public void enemyHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);

        if(presentHealth <= 0)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("AimRun", false);
            animator.SetBool("Shoot", false);
            animator.SetBool("AimDie", true);
            animator.SetBool("Die", true);
            enemyDie();
        }
    }


    private void enemyDie()
    {
        enemyAgent.SetDestination(transform.position);
        enemySpeed = 0f;
        shootingRadius = 0f;
        visionRadius = 0f;
        playerInvisionRadius = false;
        playerInShootingRadius = false;
        Object.Destroy(gameObject, 5.0f);
    }


}
