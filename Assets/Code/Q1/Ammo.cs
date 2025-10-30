using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace finalProject
{
    public class Ammo : MonoBehaviour
    {
        [Header("Ammo Settings")]
        [SerializeField] int maxAmmo = 100;
        [SerializeField] int currentAmmo = 100;

        [Header("UI References")]
        [SerializeField] bool rotateDial = false;
        [SerializeField] RectTransform dialTransform;
        [SerializeField] Image fillImage;
        [SerializeField] float fullAngle = 0f;
        [SerializeField] float emptyAngle = -360f;
        [SerializeField] TextMeshProUGUI ammoText;

        public int CurrentAmmo => currentAmmo;
        public int MaxAmmo => maxAmmo;
        public bool HasAmmo => currentAmmo > 0;

        void Awake()
        {
            maxAmmo = Mathf.Clamp(maxAmmo, 0, 100);
            if (maxAmmo < 0)
            {
                maxAmmo = 0;
            }

            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
            UpdateUI();
        }

        public void ApplyStoredAmmo(int storedAmmo)
        {
            currentAmmo = Mathf.Clamp(storedAmmo, 0, maxAmmo);
            SyncStats();
            UpdateUI();
        }

        public bool TrySpendAmmo(int amount)
        {
            if (amount <= 0)
            {
                return true;
            }

            if (currentAmmo < amount)
            {
                UpdateUI();
                return false;
            }

            currentAmmo -= amount;
            SyncStats();
            UpdateUI();
            return true;
        }

        public void AddAmmo(int amount)
        {
            if (amount <= 0 || maxAmmo <= 0)
            {
                return;
            }

            currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
            SyncStats();
            UpdateUI();
        }

        public void Refill()
        {
            currentAmmo = maxAmmo;
            SyncStats();
            UpdateUI();
        }

        public void SetAmmo(int amount)
        {
            currentAmmo = Mathf.Clamp(amount, 0, maxAmmo);
            SyncStats();
            UpdateUI();
        }

        void SyncStats()
        {
            if (PlayerStats.Initialized)
            {
                PlayerStats.UpdateAmmo(currentAmmo);
            }
        }

        void UpdateUI()
        {
            if (ammoText != null)
            {
                ammoText.text = currentAmmo.ToString();
            }

            if (rotateDial && dialTransform != null)
            {
                float t = maxAmmo <= 0 ? 0f : (float)currentAmmo / maxAmmo;
                float angle = Mathf.Lerp(emptyAngle, fullAngle, t);
                dialTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }

            if (fillImage != null)
            {
                float t = maxAmmo <= 0 ? 0f : (float)currentAmmo / maxAmmo;
                fillImage.fillAmount = t;
            }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (!Application.isPlaying)
            {
                maxAmmo = Mathf.Clamp(maxAmmo, 0, 100);

                currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
                UpdateUI();
            }
        }
#endif
    }
}
