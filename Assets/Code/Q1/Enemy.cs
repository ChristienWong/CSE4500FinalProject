using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace finalProject
{
    public class Enemy : MonoBehaviour
    {
        public Animator _animator;
        public int maxHealth = 3;
        private int currentHealth;
        private Animator anim;

        void Start()
        {
            currentHealth = maxHealth;
            anim = GetComponent<Animator>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(1); // or whatever amount you want
                }
            }
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            Debug.Log("Current health: " + currentHealth);

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

