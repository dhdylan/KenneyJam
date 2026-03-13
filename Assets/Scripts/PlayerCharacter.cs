using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(MovementController2D), typeof(PlayerCombatController))]
public class PlayerCharacter : Character
{
    [SerializeField] private Animator _animator;
    [SerializeField] protected float _postHitInvulnerabilityTime = 0.2f;

    private MovementController2D _movementController2D;
    private PlayerCombatController _combatController;

    private Coroutine _preventPlayerMovementFromAttackCoroutine;

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();

        _movementController2D = GetComponent<MovementController2D>();
        _combatController = GetComponent<PlayerCombatController>();
    }

    protected override void Start()
    {
        base.Start();
        _hurtbox.SetPostHitInvulnerabilityTime(_postHitInvulnerabilityTime);
        _hurtbox.OnHurt.AddListener(OnPlayerHurt);
        _health.OnDeath.AddListener(OnPlayerDied);
        _combatController.OnBasicAttack.AddListener(OnPlayerBasicAttack);
    }
    #endregion

    #region Getters Setters
    public MovementController2D GetMovementController() { return _movementController2D; } 
    public PlayerCombatController GetCombatController() { return _combatController; }
    #endregion


    public void ResetForRespawn()
    {
        _rigidbody2D.linearVelocity = Vector2.zero;
        _health.SetHealth(_health.GetMaxHealth());
    }

    #region Event Handlers
    private void OnPlayerDied()
    {
        GameManager.instance.OnPlayerDied();
    }

    private void OnPlayerHurt(Hit hit)
    {
        Vector3 directionFromHit = (transform.position - hit.instigator.transform.position).normalized;
        _rigidbody2D.AddForce(directionFromHit * hit.knockbackAmount, ForceMode2D.Impulse);

        _health.AdjustHealth(-hit.damage);
        Debug.Log($"Player hurt for {hit.damage} damage by {hit.instigator.name}. Remaining health: {_health.GetCurrentHealth()}");
    }

    private void OnPlayerBasicAttack()
    {
        if(_preventPlayerMovementFromAttackCoroutine != null)
        {
            StopCoroutine(_preventPlayerMovementFromAttackCoroutine);
        }

        _preventPlayerMovementFromAttackCoroutine = StartCoroutine(PreventPlayerMovementFromAttack());
    }

    private IEnumerator PreventPlayerMovementFromAttack()
    {
        _movementController2D.SetCanMove(false);
        yield return new WaitForSeconds(_combatController.GetBasicAttackMovementStopTime());
        _movementController2D.SetCanMove(true);
    }
    #endregion
}