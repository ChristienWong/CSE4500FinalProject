using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace finalProject
{
    public class PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 5;
        private int currentHealth;

        [SerializeField]
        private Heart heartUI;

        [SerializeField]
        Respawn _respawn;

        void Start()
        {
            if (heartUI == null)
            {
                heartUI = FindObjectOfType<Heart>();
            }

            if (_respawn == null)
            {
                _respawn = GetComponent<Respawn>();
            }

            ResetHealth();
        }

        public void TakeDamage(int amount)
        {
            currentHealth = Mathf.Max(0, currentHealth - amount);
            Debug.Log("Player damaged. Current health: " + currentHealth);

            SyncHeartUI();

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            SyncHeartUI();
        }

        void SyncHeartUI()
        {
            if (heartUI == null)
            {
                return;
            }

            heartUI.numOfHearts = maxHealth;
            heartUI.health = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        void Die()
        {
            Debug.Log("Player died");

            if (_respawn != null)
            {
                _respawn.DoRespawn();
            }
            else
            {
                Debug.LogWarning("Player died but no Respawn component was found to handle respawn.");
            }
        }
    }
}
