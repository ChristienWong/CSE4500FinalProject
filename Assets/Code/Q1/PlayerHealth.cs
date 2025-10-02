using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

namespace finalProject
{
    public class PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 5;
        private int currentHealth;

        void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            Debug.Log("Player damaged. Current health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Debug.Log("Player died");
        }
    }
}
