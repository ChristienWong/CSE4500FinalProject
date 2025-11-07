using System.Collections;
using UnityEngine;

namespace finalProject
{
    public class DamageFlash : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] targetRenderers;
        [SerializeField] Color flashColor = new Color(1f, 0.2f, 0.2f);
        [SerializeField] float flashDuration = 0.1f;

        Coroutine flashRoutine;
        Color[] originalColors = System.Array.Empty<Color>();

        void Awake()
        {
            if (targetRenderers == null || targetRenderers.Length == 0)
            {
                targetRenderers = GetComponentsInChildren<SpriteRenderer>();
            }

            CacheOriginalColors();
        }

        void CacheOriginalColors()
        {
            if (targetRenderers == null)
            {
                originalColors = System.Array.Empty<Color>();
                return;
            }

            originalColors = new Color[targetRenderers.Length];
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                originalColors[i] = targetRenderers[i] != null ? targetRenderers[i].color : Color.white;
            }
        }

        public void TriggerFlash()
        {
            if (!isActiveAndEnabled || targetRenderers == null || targetRenderers.Length == 0)
            {
                return;
            }

            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }

            flashRoutine = StartCoroutine(FlashSequence());
        }

        IEnumerator FlashSequence()
        {
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] == null)
                {
                    continue;
                }

                originalColors[i] = targetRenderers[i].color;
                targetRenderers[i].color = flashColor;
            }

            yield return new WaitForSeconds(flashDuration);

            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] == null)
                {
                    continue;
                }

                targetRenderers[i].color = originalColors[i];
            }

            flashRoutine = null;
        }

        void OnDisable()
        {
            if (targetRenderers == null)
            {
                return;
            }

            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] == null)
                {
                    continue;
                }

                targetRenderers[i].color = originalColors != null && i < originalColors.Length
                    ? originalColors[i]
                    : Color.white;
            }
        }
    }
}
