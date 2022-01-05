
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Variables
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;
	public float bulletsNumber;
    public float maxBullets = 30f;
    public float bulletsPerLoader;
    
    public float nextTimeToFire = 0f;
    private float reloadTime = 1f;
    public bool isReloading = false;

    public float recoilUp = 5f;
    public float recoilRight = 2f;
    
    
    //references
    private MouseLook mouseLook;
    public Transform firePoint;
    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public Animator animator;
    

    private void Start()
    {
        bulletsPerLoader = Mathf.Clamp(bulletsPerLoader, 0f, 30f);
        bulletsNumber = bulletsPerLoader;
        mouseLook = fpsCamera.GetComponent<MouseLook>();
    }

    private void Update()
    {
        if (isReloading)
        {
            if (bulletsNumber == 0)
                maxBullets -= bulletsPerLoader;
            else
                maxBullets -= bulletsPerLoader - bulletsNumber;
        
            if (maxBullets < bulletsPerLoader)
                bulletsNumber = maxBullets;
            else
                bulletsNumber = bulletsPerLoader;

            if (maxBullets <= 0)
            {
                maxBullets = 0f;
                bulletsNumber = bulletsPerLoader;
            }
        }
        
        
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
    

    public IEnumerator Reload()
    {
        isReloading = true;

        animator.SetBool("Reloading", true);
        
        yield return new WaitForSeconds(reloadTime - .25f);
        
        animator.SetBool("Reloading", false);
        
        yield return new WaitForSeconds(.25f);
        
       
        isReloading = false;
    }

    public void Shoot()
    {
        muzzleFlash.Play();
        mouseLook.GetRecoil(recoilUp / 5f, recoilRight / 5f);
		bulletsNumber--;
        
        RaycastHit hit;
        //Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * range, Color.green, 1f);
        if (Physics.Raycast(firePoint.position, fpsCamera.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            Boss boss = hit.transform.GetComponent<Boss>();
            Normal normal = hit.transform.GetComponent<Normal>();

            if (target != null)
                target.TakeDamage(damage);

            if (boss != null)
                boss.TakeDamage(damage);

            if (normal !=null)
            {
                FindObjectOfType<AudioManager>().Play("dead");
                Debug.Log("You Loose");
                Time.timeScale = 0;
            }

            if (hit.rigidbody != null)
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }

        
    }
    
}
