using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Health))]
public class Character : MonoBehaviour
{
    // TODO:  Need to just get rid of these C# properties altogether and replace them with concrete getters and setters.
    new protected Rigidbody2D rigidbody2D;
    protected Health health;

    [SerializeField] protected SpriteRenderer mainSpriteRenderer;

    protected virtual void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    public Health GetHealth() { return health; }

    public Rigidbody2D GetRigidbody2D() { return rigidbody2D; }
}