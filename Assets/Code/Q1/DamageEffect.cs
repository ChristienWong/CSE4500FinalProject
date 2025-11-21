using UnityEngine;

namespace finalProject
{
    public class DamageEffect : MonoBehaviour
    {
        DamageFlash flash;

        void Awake()
        {
            
            flash = GetComponent<DamageFlash>();
        }

        public void PlayEffects()
        {
          
            if (CameraShake.instance != null)
            {
                CameraShake.instance.Shake(0.15f, 0.2f);
            }

           
            if (flash != null)
            {
                flash.TriggerFlash();
            }
        }
    }
}
