using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    Vector3 originalPos;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        originalPos = transform.localPosition;
    }

    public void Shake(float duration = 0.2f, float magnitude = 0.2f)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    IEnumerator DoShake(float duration, float magnitude)
    {
        float time = 0f;

        while (time < duration)
        {
            transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * magnitude;
            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
