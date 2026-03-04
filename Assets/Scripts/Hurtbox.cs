using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Hurtbox : MonoBehaviour
{
    public UnityEvent<Hit> OnHurt;
    
    public void Hurt(Hit hit)
    {
        if (hit == null)
        {
            Debug.LogError("Hurtbox received a null Hit.", this);
        }
        else
        {
            OnHurt.Invoke(hit);
        }
    }
}