using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace finalProject
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ammoText;
        [SerializeField] Ammo ammoSystem;

        void Update()
        {
            if (ammoSystem != null)
            {
                ammoText.text = $"Ammo: {ammoSystem.CurrentAmmo}";
            }
        }
    }
}