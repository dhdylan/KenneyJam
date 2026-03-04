using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacter : Character
{
    [SerializeField] private Hitbox basicAttackHitbox;
    [SerializeField] private Hurtbox hurtbox;
    
    private MovementController2D movementController2D;

    private Coroutine basicAttackCoroutine;

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();

        movementController2D = GetComponent<MovementController2D>();
    }

    protected void Start()
    {
        hurtbox.OnHurt.AddListener(OnPlayerHurt);
        health.OnDeath.AddListener(OnPlayerDied);
    } 
    #endregion

    #region Getters Setters
    public MovementController2D GetMovementController() { return movementController2D; } 
    #endregion

    public void BasicAttack()
    {
        if(basicAttackCoroutine != null)
        {
            Debug.LogWarning("Basic attack already in progress. Restarting it.", this);
            StopCoroutine(basicAttackCoroutine);
        }

        basicAttackCoroutine = StartCoroutine(BasicAttackCoroutine());
    }

    private IEnumerator BasicAttackCoroutine()
    {
        // TODO: Play attack animation here
        // Enable hitbox for a short duration
        basicAttackHitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        basicAttackHitbox.gameObject.SetActive(false);
    }

    public void ResetForRespawn()
    {
        rigidbody2D.linearVelocity = Vector2.zero;
        health.SetHealth(health.GetMaxHealth());
    }

    #region Event Handlers
    private void OnPlayerDied()
    {
        GameManager.instance.OnPlayerDied();
    }

    private void OnPlayerHurt(Hit hit)
    {
        Vector3 directionFromHit = (transform.position - hit.instigator.transform.position).normalized;
        rigidbody2D.AddForce(directionFromHit * hit.knockbackAmount, ForceMode2D.Impulse);

        health.AdjustHealth(-hit.damage);
        Debug.Log($"Player hurt for {hit.damage} damage by {hit.instigator.name}. Remaining health: {health.GetCurrentHealth()}");
    } 
    #endregion
}