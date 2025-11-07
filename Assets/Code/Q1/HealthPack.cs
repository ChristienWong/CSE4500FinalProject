using UnityEngine;

namespace finalProject
{
    public class HealthPack : MonoBehaviour
    {
        [SerializeField]
        private int healAmount = 1;

        [SerializeField]
        private AudioClip pickupSound;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(healAmount);

                    if (pickupSound != null)
                        AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                    Destroy(gameObject); 
                }
            }
        }
    }
}

