using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour
{
    public Hit currentHit;

    private void Awake()
    {
        currentHit.instigator = gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enabled && collision.TryGetComponent<Hurtbox>(out Hurtbox hurtbox))
        {
            if (currentHit == null)
            {
                Debug.LogError("Hitbox has no Hit assigned.", this);
            }
            else
            {
                hurtbox.Hurt(currentHit);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enabled && collision.TryGetComponent<Hurtbox>(out Hurtbox hurtbox))
        {
            if (currentHit == null)
            {
                Debug.LogError("Hitbox has no Hit assigned.", this);
            }
            else
            {
                hurtbox.Hurt(currentHit);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (enabled && collision.gameObject.TryGetComponent<Hurtbox>(out Hurtbox hurtbox))
        {
            if (currentHit == null)
            {
                Debug.LogError("Hitbox has no Hit assigned.", this);
            }
            else
            {
                hurtbox.Hurt(currentHit);
            }
        }
    }
}