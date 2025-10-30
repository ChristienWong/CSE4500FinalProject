using UnityEngine;

namespace finalProject
{
    public class AmmoPack : MonoBehaviour
    {
        [SerializeField] int refillAmount = 20;
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            Ammo ammo = other.GetComponentInChildren<Ammo>() ?? other.GetComponent<Ammo>();

            if (ammo == null)
            {
                ammo = FindObjectOfType<Ammo>();
            }

            if (ammo == null)
            {
                return;
            }

            int clampedRefill = Mathf.Max(0, refillAmount);
            if (clampedRefill <= 0)
            {
                return;
            }

            ammo.AddAmmo(clampedRefill);

            Destroy(gameObject);
        }
    }
}
