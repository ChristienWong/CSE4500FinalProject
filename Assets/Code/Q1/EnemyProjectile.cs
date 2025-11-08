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
            }
        }

        public void Initialize(int damageAmount)
        {
            damage = Mathf.Max(1, damageAmount);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }

            if (other.CompareTag("Player"))
            {
                PlayerHealth player = other.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }

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
