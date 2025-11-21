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
            // --------- PLAY SOUNDS ---------
            // Hit an enemy or player  → "hit" sound
            if (other.CompareTag("Enemy") || other.CompareTag("Player"))
            {
                SoundManager.instance.PlaySoundHit();
            }
            // Hit the ground (or whatever layer you use for it) → "miss" sound
            else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                SoundManager.instance.PlaySoundMiss();
            }
            // --------------------------------
            
            PlayerHealth player = FindObjectOfType<PlayerHealth>();

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


            //get damaged if hurt by enemy projectile
            if (other.CompareTag("Player")){
                Debug.Log("Player should take damage.");
                PlayerHealth playerH = FindObjectOfType<PlayerHealth>();
                playerH.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }
}

