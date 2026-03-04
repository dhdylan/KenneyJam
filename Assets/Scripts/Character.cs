using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(MovementController2D), typeof(Health))]
public class Character : MonoBehaviour
{
    // TODO:  Need to just get rid of these C# properties altogether and replace them with concrete getters and setters.
    new public Rigidbody2D rigidbody2D { get; private set; }
    public MovementController2D movementController2D { get; private set; }
    public Health health { get; private set; }

    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        movementController2D = GetComponent<MovementController2D>();
        health = GetComponent<Health>();
    }
}