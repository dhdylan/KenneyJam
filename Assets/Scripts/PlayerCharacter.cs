using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(MovementController2D), typeof(PlayerCombatController))]
public class PlayerCharacter : Character
{
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private Animator animator;
    
    private MovementController2D movementController2D;
    private PlayerCombatController combatController;

    private Coroutine preventPlayerMovementFromAttackCoroutine;

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();

        movementController2D = GetComponent<MovementController2D>();
        combatController = GetComponent<PlayerCombatController>();
    }

    protected void Start()
    {
        hurtbox.OnHurt.AddListener(OnPlayerHurt);
        health.OnDeath.AddListener(OnPlayerDied);
        combatController.OnBasicAttack.AddListener(OnPlayerBasicAttack);
    } 
    #endregion

    #region Getters Setters
    public MovementController2D GetMovementController() { return movementController2D; } 
    public PlayerCombatController GetCombatController() { return combatController; }
    #endregion


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

    private void OnPlayerBasicAttack()
    {
        if(preventPlayerMovementFromAttackCoroutine != null)
        {
            StopCoroutine(preventPlayerMovementFromAttackCoroutine);
        }

        preventPlayerMovementFromAttackCoroutine = StartCoroutine(PreventPlayerMovementFromAttack());
    }

    private IEnumerator PreventPlayerMovementFromAttack()
    {
        movementController2D.SetCanMove(false);
        yield return new WaitForSeconds(combatController.GetBasicAttackMovementStopTime());
        movementController2D.SetCanMove(true);
    }
    #endregion
}