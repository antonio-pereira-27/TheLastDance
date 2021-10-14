using System;
using UnityEngine;

public class Target : MonoBehaviour
{
   // variáveis
   public float health = 100f;
   public float currentHealth;

   public HealthBar healthBar;

   // função de inicio 
   private void Start()
   {
      currentHealth = health;
     healthBar.SetMaxHealth(health);
   }
   

   public void TakeDamage(float amount)
   {
      currentHealth -= amount;
      healthBar.SetHealth(currentHealth);
      if (currentHealth <= 0f)
      {
         Die();
      }
   }

   void Die()
   {
      Destroy(gameObject);
   }
}
