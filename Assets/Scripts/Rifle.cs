using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera camera;
    public float giveDamageOf = 10f;
    public float shooringRange = 100f;
    public float fireCharge = 15f;
    public Animator animator;
    public PlayerScript player;

    [Header("Rifle Ammount and shooting")]
    public int maxAmmount = 20;
    public int mag = 15;
    public int presentAmmount;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;
    public float nextTimeShoot = 0f;


    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject impactEffect;
    public GameObject bloodEffect;
    public GameObject droneEffect;


    [Header("Sounds and UI")]
    public GameObject AmmoOutUI;
    public int timeToShowUi = 1;
    public AudioClip shootingSound;
    public AudioClip reloadingSound;
    public AudioSource audioSource;



    private void Awake()
    {
        presentAmmount = maxAmmount;
    }

    void Update()
    {
        if (setReloading)
        {
            return;
        }

        if(presentAmmount <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeShoot)
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
            nextTimeShoot = Time.time + 1f / fireCharge;
            Shoot();
        }
        else if(Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }
        else if(Input.GetButton("Fire2") && Input.GetButton("Fire1"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("FireWalk", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Reloading", false);
        }
      
        else
        {
            animator.SetBool("Fire", false);
            animator.SetBool("Idle", true);
            animator.SetBool("FireWalk", false);
            animator.SetBool("Reloading", false);
            


        }
        
    }

    void Shoot()
    {
        if(mag == 0)
        {
            //show ammo text
            StartCoroutine(ShowAmmoOut());
            return;
        }

        presentAmmount--;

        if(presentAmmount == 0)
        {
            mag--;
        }


        //Updating Ui
        UiAmmoCount.occurrence.UpdateAmmoText(presentAmmount);
        UiAmmoCount.occurrence.UpdateMagText(mag);



        muzzleSpark.Play();
        audioSource.PlayOneShot(shootingSound);

        RaycastHit hitInfo;

        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo, shooringRange))
        {
            Debug.Log(hitInfo.transform.name);


            Objects objects = hitInfo.transform.GetComponent<Objects>();
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            Drone drone = hitInfo.transform.GetComponent<Drone>();


            if(objects != null)
            {
                objects.ObjectHitDamage(giveDamageOf);
                GameObject impactGo = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGo, 1f);
            }
            else if(enemy !=null)
            {
                enemy.enemyHitDamage(giveDamageOf);
                GameObject impactGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGo, 2f);

            }
            else if(drone != null)
            {
                drone.droneHitDamage(giveDamageOf);
                GameObject impactGo = Instantiate(droneEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGo, 2f);
            }
            

        }
    }

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("reloading");
        animator.SetBool("Reloading", true);
        audioSource.PlayOneShot(reloadingSound);
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        presentAmmount = maxAmmount;
        player.playerSpeed = 7.0f;
        player.playerSprint = 3f;
        setReloading = false;
    }


    IEnumerator ShowAmmoOut()
    {
        AmmoOutUI.SetActive(true);
        yield return new WaitForSeconds(timeToShowUi);
        AmmoOutUI.SetActive(false);
    }
}
