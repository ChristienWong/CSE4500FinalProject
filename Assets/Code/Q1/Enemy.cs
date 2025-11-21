using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace finalProject
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] Animator animator;
        public int maxHealth = 3;
        [SerializeField] int contactDamage = 1;
        [SerializeField] float damageCooldown = 0.75f;
        [SerializeField] string idleStateName = "EnemyIdle";

        int currentHealth;
        float lastDamageTime = -Mathf.Infinity;
        int idleStateHash;
        [SerializeField] DamageFlash damageFlash;

        void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            if (!string.IsNullOrEmpty(idleStateName))
            {
                idleStateHash = Animator.StringToHash(idleStateName);
            }
        }

        void OnEnable()
        {
            EnsureIdleAnimationPlaying();
        }

        void Start()
        {
            currentHealth = maxHealth;

            if (damageFlash == null)
            {
                damageFlash = GetComponent<DamageFlash>();
            }
        }

        void EnsureIdleAnimationPlaying()
        {
            if (animator == null)
            {
                Debug.LogWarning($"{name} is missing an Animator component so it cannot play idle animations.");
                return;
            }

            animator.enabled = true;
            animator.speed = Mathf.Approximately(animator.speed, 0f) ? 1f : animator.speed;
            animator.Rebind();
            animator.Update(0f);

            bool hasController = animator.runtimeAnimatorController != null;
            bool canPlayIdle = hasController &&
                               idleStateHash != 0 &&
                               animator.HasState(0, idleStateHash);

            if (canPlayIdle)
            {
                animator.Play(idleStateHash, 0, 0f);
            }
            else if (hasController)
            {
                animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0f);
            }
            else
            {
                Debug.LogWarning($"{name} does not have a RuntimeAnimatorController assigned and cannot animate.");
            }
        }

        void OnCollisionEnter2D(Collision2D collision) => TryDamagePlayer(collision);

        void OnCollisionStay2D(Collision2D collision) => TryDamagePlayer(collision);

        public void TryDamagePlayer(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                return;
            }

            PlayerHealth player = FindObjectOfType<PlayerHealth>();
            if (player == null || Time.time < lastDamageTime + damageCooldown)
            {
                return;
            }

            player.TakeDamage(Mathf.Max(1, contactDamage));
            lastDamageTime = Time.time;
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            Debug.Log("Current health: " + currentHealth);
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

