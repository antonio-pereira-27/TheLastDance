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

    public float nextTimeToFire = 0f;
    private float reloadTime = 1f;
    public bool isReloading = false;

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
    

    public IEnumerator Reload()
    {
        isReloading = true;
        
        animator.SetBool("Reloading", true);
        
        yield return new WaitForSeconds(reloadTime - .25f);
        
        animator.SetBool("Reloading", false);
        
        yield return new WaitForSeconds(.25f);
        
        bulletsNumber = maxBullets;
        isReloading = false;
    }

    public void Shoot()
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
