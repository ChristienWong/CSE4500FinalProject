using UnityEngine;

namespace finalProject
{
    public class EnemyShooter : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] int maxHealth = 3;
        [SerializeField] DamageFlash damageFlash;

        [Header("Contact Damage")]
        [SerializeField] int contactDamage = 1;
        [SerializeField] float damageCooldown = 0.75f;

        [Header("Ranged Attack")]
        [SerializeField] Transform firePoint;
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] float projectileSpeed = 6f;
        [SerializeField] float fireInterval = 1.5f;
        [SerializeField] float detectionRange = 10f;

        Animator animator;
        PlayerHealth targetPlayer;
        int currentHealth;
        float lastDamageTime = -Mathf.Infinity;
        float lastShotTime = -Mathf.Infinity;

        void Awake()
        {
            animator = GetComponent<Animator>();
            currentHealth = Mathf.Max(1, maxHealth);

            if (damageFlash == null)
            {
                damageFlash = GetComponent<DamageFlash>();
            }

            if (firePoint == null)
            {
                firePoint = transform;
            }
        }

        void Start()
        {
            AcquireTarget();
        }

        void Update()
        {
            if (targetPlayer == null)
            {
                AcquireTarget();
                return;
            }

            float sqrRange = detectionRange * detectionRange;
            float sqrDistance = (targetPlayer.transform.position - transform.position).sqrMagnitude;

            if (sqrDistance <= sqrRange)
            {
                TryShoot();
            }
        }

        void AcquireTarget()
        {
            PlayerHealth player = FindObjectOfType<PlayerHealth>();
            if (player != null)
            {
                targetPlayer = player;
            }
        }

        void TryShoot()
        {
            if (projectilePrefab == null)
            {
                return;
            }

            if (Time.time < lastShotTime + fireInterval)
            {
                return;
            }

            lastShotTime = Time.time;

            Vector2 direction = (targetPlayer.transform.position - firePoint.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            if (projectile.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.velocity = direction * projectileSpeed;
            }

            if (projectile.TryGetComponent<EnemyProjectile>(out var enemyProjectile))
            {
                enemyProjectile.Initialize(Mathf.Max(1, contactDamage));
            }

            if (animator != null)
            {
                animator.SetTrigger("Shoot");
            }
        }

        void OnCollisionEnter2D(Collision2D collision) => TryDamagePlayer(collision);

        void OnCollisionStay2D(Collision2D collision) => TryDamagePlayer(collision);

        void TryDamagePlayer(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                return;
            }

            if (Time.time < lastDamageTime + damageCooldown)
            {
                return;
            }

            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player == null)
            {
                return;
            }

            player.TakeDamage(Mathf.Max(1, contactDamage));
            lastDamageTime = Time.time;
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= Mathf.Max(1, amount);

            if (damageFlash != null)
            {
                damageFlash.TriggerFlash();
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }
}
