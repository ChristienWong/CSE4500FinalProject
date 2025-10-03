using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatformSimple : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float travelTime = 2.0f;   

    Rigidbody2D rb;
    Vector2 a, b;     
    float t;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

    
        if (pointA) a = pointA.position;
        if (pointB) b = pointB.position;
    }

    void FixedUpdate(){
        if (travelTime <= 0f || pointA == null || pointB == null) return;
        t += Time.fixedDeltaTime / travelTime;     
        float u = Mathf.PingPong(t, 1f);           
        rb.MovePosition(Vector2.Lerp(a, b, u));
    }
}
