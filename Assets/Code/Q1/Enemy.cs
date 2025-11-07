using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace finalProject
{
    public class Enemy : MonoBehaviour
    {
        public Animator _animator;
        public int maxHealth = 3;
        [SerializeField] int contactDamage = 1;
        [SerializeField] float damageCooldown = 0.75f;

        int currentHealth;
        float lastDamageTime = -Mathf.Infinity;
        Animator anim;
        [SerializeField] DamageFlash damageFlash;

        void Start()
        {
            currentHealth = maxHealth;
            anim = GetComponent<Animator>();
            if (damageFlash == null)
            {
                damageFlash = GetComponent<DamageFlash>();
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

            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
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

