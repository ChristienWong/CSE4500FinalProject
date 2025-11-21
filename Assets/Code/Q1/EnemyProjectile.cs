using UnityEngine;

namespace finalProject
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] float lifetimeSeconds = 4f;
        [SerializeField] GameObject impactEffect;

        int damage = 1;

        void Start()
        {
            if (lifetimeSeconds > 0f)
            {
                Destroy(gameObject, lifetimeSeconds);
                Debug.Log("Player should take damage.");
            }
        }

        public void Initialize(int damageAmount)
        {
            damage = Mathf.Max(1, damageAmount);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Player should take damage.");
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }

            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player == null)
            {
                player = other.GetComponentInParent<PlayerHealth>();
            }

            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }

            if (!other.isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}
