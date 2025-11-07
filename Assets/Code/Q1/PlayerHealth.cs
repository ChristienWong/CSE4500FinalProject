using UnityEngine;
using UnityEngine.SceneManagement;

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

        [SerializeField]
        Ammo ammoSystem;

        [SerializeField]
        DamageFlash damageFlash;

        [SerializeField]
        int respawnAmmo = 50;

        [SerializeField]
        string restartSceneName = "Q1";

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

            if (ammoSystem == null)
            {
                ammoSystem = FindObjectOfType<Ammo>();
            }

            if (damageFlash == null)
            {
                damageFlash = GetComponent<DamageFlash>();
            }

            InitializeStats();
            ApplyStatsToWorld();
        }

        public void TakeDamage(int amount)
        {
            currentHealth = Mathf.Max(0, currentHealth - amount);
            Debug.Log("Player damaged. Current health: " + currentHealth);
            TriggerDamageFeedback();

            PlayerStats.UpdateHealth(currentHealth);
            SyncHeartUI();

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            PlayerStats.UpdateHealth(currentHealth);
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

            PlayerStats.ResetForDeath();

            if (ammoSystem != null)
            {
                ammoSystem.ApplyStoredAmmo(PlayerStats.CurrentAmmo);
            }

            if (string.IsNullOrWhiteSpace(restartSceneName))
            {
                restartSceneName = SceneManager.GetActiveScene().name;
            }

            SceneManager.LoadScene(restartSceneName);
        }

        void InitializeStats()
        {
            int ammoMax = ammoSystem != null ? ammoSystem.MaxAmmo : respawnAmmo;
            int ammoCurrent = ammoSystem != null ? ammoSystem.CurrentAmmo : respawnAmmo;

            if (!PlayerStats.Initialized)
            {
                PlayerStats.Initialize(maxHealth, maxHealth, ammoMax, ammoCurrent, respawnAmmo);
            }
            else
            {
                PlayerStats.UpdateMaxValues(maxHealth, ammoMax, respawnAmmo);
            }
        }

        void ApplyStatsToWorld()
        {
            currentHealth = Mathf.Clamp(PlayerStats.CurrentHealth, 0, maxHealth);
            SyncHeartUI();

            if (ammoSystem != null && PlayerStats.Initialized)
            {
                ammoSystem.ApplyStoredAmmo(PlayerStats.CurrentAmmo);
            }
        }

        void TriggerDamageFeedback()
        {
            if (damageFlash != null)
            {
                damageFlash.TriggerFlash();
            }
        }
       public void Heal(int amount)
{
    currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    Debug.Log("Player healed. Current health: " + currentHealth);

    PlayerStats.UpdateHealth(currentHealth);
    SyncHeartUI();
}

    }
}
