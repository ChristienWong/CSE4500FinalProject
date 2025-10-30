using UnityEngine;

namespace finalProject
{
    public static class PlayerStats
    {
        public static bool Initialized { get; private set; }
        public static int MaxHealth { get; private set; }
        public static int CurrentHealth { get; private set; }
        public static int MaxAmmo { get; private set; }
        public static int CurrentAmmo { get; private set; }
        public static int RespawnAmmo { get; private set; }

        public static void Initialize(int maxHealth, int startHealth, int maxAmmo, int startAmmo, int respawnAmmo)
        {
            MaxHealth = Mathf.Max(0, maxHealth);
            MaxAmmo = Mathf.Clamp(maxAmmo, 0, 100);
            RespawnAmmo = Mathf.Clamp(respawnAmmo, 0, MaxAmmo);

            CurrentHealth = Mathf.Clamp(startHealth, 0, MaxHealth);
            CurrentAmmo = Mathf.Clamp(startAmmo, 0, MaxAmmo);

            Initialized = true;
        }

        public static void UpdateMaxValues(int maxHealth, int maxAmmo, int respawnAmmo)
        {
            MaxHealth = Mathf.Max(0, maxHealth);
            MaxAmmo = Mathf.Clamp(maxAmmo, 0, 100);
            RespawnAmmo = Mathf.Clamp(respawnAmmo, 0, MaxAmmo);

            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            CurrentAmmo = Mathf.Clamp(CurrentAmmo, 0, MaxAmmo);
        }

        public static void UpdateHealth(int value)
        {
            CurrentHealth = Mathf.Clamp(value, 0, MaxHealth);
        }

        public static void UpdateAmmo(int value)
        {
            CurrentAmmo = Mathf.Clamp(value, 0, MaxAmmo);
        }

        public static void ResetForDeath()
        {
            CurrentHealth = MaxHealth;
            CurrentAmmo = RespawnAmmo;
        }
    }
}
