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
        [SerializeField] string shootTriggerName = "Shoot";
        [SerializeField] string shootAnimationStateName = "EnemyShoot";

        Animator animator;
        PlayerHealth targetPlayer;
        int currentHealth;
        float lastDamageTime = -Mathf.Infinity;
        float lastShotTime = -Mathf.Infinity;
        Vector3 initialScale;
        Collider2D[] ownColliders;
        int shootStateHash;

        void Awake()
        {
            animator = GetComponent<Animator>();
            currentHealth = Mathf.Max(1, maxHealth);
            initialScale = transform.localScale;
            ownColliders = GetComponentsInChildren<Collider2D>();
            shootStateHash = !string.IsNullOrEmpty(shootAnimationStateName)
                ? Animator.StringToHash(shootAnimationStateName)
                : 0;

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

            if (!TryGetAimDirection(out Vector2 direction))
            {
                return;
            }

            // Always keep the sprite facing the player even if out of range.
            UpdateFacing(direction);

            if (sqrDistance <= sqrRange)
            {
                AimAtTarget(direction);
                TryShoot(direction);
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

        bool TryGetAimDirection(out Vector2 direction)
        {
            direction = Vector2.zero;

            if (targetPlayer == null || firePoint == null)
            {
                return false;
            }

            Vector2 delta = targetPlayer.transform.position - firePoint.position;
            if (delta.sqrMagnitude <= 0.0001f)
            {
                return false;
            }

            direction = delta.normalized;
            return true;
        }

        void UpdateFacing(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) <= 0.9f)
            {
                return;
            }

            float desiredSign = direction.x >= 0f ? 1f : -1f;
            float initialSign = Mathf.Approximately(initialScale.x, 0f) ? 1f : Mathf.Sign(initialScale.x);
            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Abs(initialScale.x) * desiredSign * initialSign;
            transform.localScale = newScale;
        }

        void AimAtTarget(Vector2 direction)
        {
            if (firePoint == null)
            {
                return;
            }

            Quaternion rotation = CalculateAimRotation(direction);
            firePoint.rotation = rotation;
        }

        void TryShoot(Vector2 direction)
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

            Quaternion projectileRotation = CalculateAimRotation(direction);
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, projectileRotation);

            if (projectile.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.velocity = direction * projectileSpeed;
            }

            IgnoreSelfCollision(projectile);

            if (projectile.TryGetComponent<EnemyProjectile>(out var enemyProjectile))
            {
                
                enemyProjectile.Initialize(Mathf.Max(1, contactDamage));
            }

            PlayShootAnimation();
        }

        Quaternion CalculateAimRotation(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        void PlayShootAnimation()
        {
            if (animator == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(shootTriggerName) && HasAnimatorTrigger(shootTriggerName))
            {
                animator.SetTrigger(shootTriggerName);
                return;
            }

            if (shootStateHash != 0 && animator.HasState(0, shootStateHash))
            {
                animator.Play(shootStateHash, 0, 0f);
            }
        }

        bool HasAnimatorTrigger(string triggerToFind)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger &&
                    parameter.name == triggerToFind)
                {
                    return true;
                }
            }

            return false;
        }

        void IgnoreSelfCollision(GameObject projectile)
        {
            if (ownColliders == null || ownColliders.Length == 0)
            {
                return;
            }

            if (!projectile.TryGetComponent<Collider2D>(out var projectileCollider))
            {
                return;
            }

            foreach (Collider2D col in ownColliders)
            {
                if (col == null)
                {
                    continue;
                }

                Physics2D.IgnoreCollision(projectileCollider, col);
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
