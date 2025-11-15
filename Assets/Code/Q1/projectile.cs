using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace finalProject
{
    public class Projectile : MonoBehaviour
    {
        public float lifetime = 2f; 
        public GameObject impactEffect; 

        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }
            Debug.Log("Hit object: " + other.name + " | Tag: " + other.tag);
            if (other.CompareTag("Enemy"))
            {
                bool dealtDamage = false;

                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                    dealtDamage = true;
                }

                EnemyShooter shooter = other.GetComponent<EnemyShooter>();
                if (shooter != null)
                {
                    shooter.TakeDamage(1);
                    dealtDamage = true;
                }

                if (dealtDamage)
                {
                    Destroy(gameObject);
                    return;
                }
            }

            Destroy(gameObject);
        }
    }
}

