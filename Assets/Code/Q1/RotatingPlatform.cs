using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;   
    public bool clockwise = true;       
    public bool isActive = true;        

    void Update()
    {
        if (!isActive) return;

        float direction = clockwise ? -1f : 1f;
        transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);
    }
}
