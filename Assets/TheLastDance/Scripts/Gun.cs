using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;
	public float bulletsNumber;
    public float maxBullets = 30f;
    
    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;
    private float reloadTime = 1f;
    private bool isReloading = false;

    public Animator animator;
    
    private void Start()
    {
        bulletsNumber = maxBullets;
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;
        
        if (bulletsNumber <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && bulletsNumber > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
        
		if(Input.GetButton("Reload") && bulletsNumber < maxBullets)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        
        animator.SetBool("Reloading", true);
        
        yield return new WaitForSeconds(reloadTime - .25f);
        
        animator.SetBool("Reloading", false);
        
        yield return new WaitForSeconds(.25f);
        
        bulletsNumber = maxBullets;
        isReloading = false;
    }

    private void Shoot()
    {
        muzzleFlash.Play();
		bulletsNumber--;
        
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Debug.Log(bulletsNumber);
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }

        
    }
    
}
