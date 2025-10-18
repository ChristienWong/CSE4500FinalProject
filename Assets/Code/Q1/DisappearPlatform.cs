using UnityEngine;
using System.Collections;

public class DisappearPlatform : MonoBehaviour
{
    public float disappearDelay = 2f;   
    public float reappearDelay = 3f;   
    public bool canReappear = true;     

    private SpriteRenderer sprite;
    private Collider2D coll;
    private bool isTriggered = false;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && collision.collider.CompareTag("Player"))
        {
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        isTriggered = true;
        yield return new WaitForSeconds(disappearDelay);

        for (int i = 0; i < 5; i++)
{
    sprite.enabled = false;
    yield return new WaitForSeconds(0.1f);
    sprite.enabled = true;
    yield return new WaitForSeconds(0.1f);
}


        sprite.enabled = false;
        coll.enabled = false;

        if (canReappear)
        {
            yield return new WaitForSeconds(reappearDelay);
            sprite.enabled = true;
            coll.enabled = true;
            isTriggered = false;
        }
    }
}

