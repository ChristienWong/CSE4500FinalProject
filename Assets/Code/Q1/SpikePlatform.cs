using System.Collections.Generic;
using UnityEngine;

namespace finalProject
{
    [RequireComponent(typeof(Collider2D))]
    public class SpikePlatform : MonoBehaviour
    {
        [Header("Spike Settings")]
        [Tooltip("Damage dealt to the player on contact")]
        public int damage = 1;

        [Tooltip("Seconds before spikes deal damage again while the player stands on them")]
        public float damageCooldown = 2f;

        [Tooltip("Restart the scene when the player dies (uses PlayerHealth)")]
        public bool restartSceneOnDeath = true;

        private readonly Dictionary<Collider2D, float> nextDamageTimes = new Dictionary<Collider2D, float>();

        void Reset()
        {
            // Automatically configure collider as trigger
            var col = GetComponent<Collider2D>();
            col.isTrigger = true;

            // Optionally put spikes on Enemy layer
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            if (enemyLayer != -1) gameObject.layer = enemyLayer;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            DealDamage(other);
            nextDamageTimes[other] = Time.time + damageCooldown;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            if (!nextDamageTimes.TryGetValue(other, out float nextDamageTime))
            {
                DealDamage(other);
                nextDamageTimes[other] = Time.time + damageCooldown;
                return;
            }

            if (Time.time < nextDamageTime) return;

            DealDamage(other);
            nextDamageTimes[other] = Time.time + damageCooldown;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            nextDamageTimes.Remove(other);
        }

        private void DealDamage(Collider2D playerCollider)
        {
            PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("[Spike] Player took " + damage + " damage!");
            }
            else
            {
                Debug.LogWarning("[Spike] No PlayerHealth found on Player.");
            }
        }
    }
}
