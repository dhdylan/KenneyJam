using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Hitbox : MonoBehaviour
{
    public Hit currentHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Hurtbox>(out Hurtbox hurtbox))
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