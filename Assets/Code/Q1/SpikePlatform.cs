using UnityEngine;

namespace finalProject
{
    [RequireComponent(typeof(Collider2D))]
    public class SpikePlatform : MonoBehaviour
    {
        [Header("Spike Settings")]
        [Tooltip("Damage dealt to the player on contact")]
        public int damage = 1;

        [Tooltip("Seconds before spikes can hurt the player again")]
        public float damageCooldown = 1f;

        [Tooltip("Restart the scene when the player dies (uses PlayerHealth)")]
        public bool restartSceneOnDeath = true;

        private float lastDamageTime;

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
            // Only hurt the player
            if (!other.CompareTag("Player")) return;

            if (Time.time < lastDamageTime + damageCooldown) return;
            lastDamageTime = Time.time;

            // Call PlayerHealth so UI updates
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
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
